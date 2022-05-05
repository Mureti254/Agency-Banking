using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace OraDB
{
    public class RabbitMQService
    {
        //Handles all messaging patterns, maybe wise checking RPC for correlation ids to messages
        private bool durable = true;
        DBHandler dbhandler = new DBHandler();

        public IConnection Get_rabbitmq_connection()
        {
            ConnectionFactory connectionfactory = new ConnectionFactory()
            {
                RequestedHeartbeat = 30,
                AutomaticRecoveryEnabled = true,
                HostName = dbhandler.GetRecords("parameters", "QUEUE_HOST").Rows[0]["item_value"].ToString(),
                UserName = dbhandler.GetRecords("parameters", "QUEUE_USER").Rows[0]["item_value"].ToString(),
                Password = dbhandler.GetRecords("parameters", "QUEUE_PASSWORD").Rows[0]["item_value"].ToString()
            };

            return connectionfactory.CreateConnection();
        }

        #region remoteprocedurecallhandler
        //https://dotnetcodr.com/2014/05/05/messaging-with-rabbitmq-and-net-c-part-3-message-exchange-patterns/
        private QueueingBasicConsumer rpc_consumer;

        public void Set_up_queue_for_rpc(IModel model, string rpc_queue_name)
        {
            model.QueueDeclare(rpc_queue_name, durable, false, false, null);
        }

        public string send_rpc_message_to_queue(string message, string rpc_queue_name, IModel model, TimeSpan timeout, string rpc_response_queue_name = null)
        {
            if (string.IsNullOrEmpty(rpc_response_queue_name))
            {
                rpc_response_queue_name = model.QueueDeclare().QueueName;
            }

            if (rpc_consumer == null)
            {
                rpc_consumer = new QueueingBasicConsumer(model);
                model.BasicConsume(rpc_response_queue_name, true, rpc_consumer);
            }

            string correlationId = Guid.NewGuid().ToString();

            IBasicProperties basicProperties = model.CreateBasicProperties();
            basicProperties.ReplyTo = rpc_response_queue_name;
            basicProperties.CorrelationId = correlationId;

            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            model.BasicPublish("", rpc_queue_name, basicProperties, messageBytes);

            DateTime timeoutDate = DateTime.Now + timeout;
            while (DateTime.Now <= timeoutDate)
            {
                BasicDeliverEventArgs deliveryArguments = (BasicDeliverEventArgs)rpc_consumer.Queue.Dequeue();
                if (deliveryArguments.BasicProperties != null && deliveryArguments.BasicProperties.CorrelationId == correlationId)
                {
                    string response = Encoding.UTF8.GetString(deliveryArguments.Body);
                    return response;
                }
            }
            throw new TimeoutException("No response before the timeout period.");
        }

        public string receive_rpc_message(IModel model, string rpc_queue_name)
        {
            string message = "";
            model.BasicQos(0, 1, false);
            QueueingBasicConsumer consumer = new QueueingBasicConsumer(model);
            model.BasicConsume(rpc_queue_name, false, consumer);

            while (true)
            {
                BasicDeliverEventArgs deliveryArguments = consumer.Queue.Dequeue() as BasicDeliverEventArgs;
                message = Encoding.UTF8.GetString(deliveryArguments.Body);
                IBasicProperties replyBasicProperties = model.CreateBasicProperties();
                replyBasicProperties.CorrelationId = deliveryArguments.BasicProperties.CorrelationId;
                byte[] responseBytes = Encoding.UTF8.GetBytes(message);
                model.BasicPublish("", deliveryArguments.BasicProperties.ReplyTo, replyBasicProperties, responseBytes);
                model.BasicAck(deliveryArguments.DeliveryTag, false);
                return message;
            }

        }
        #endregion
    }

    public class RpcClient
    {
        private IConnection connection;
        private IModel channel;
        private string replyQueueName;
        private EventingBasicConsumer consumer;
        private BlockingCollection<string> respQueue = new BlockingCollection<string>();
        private IBasicProperties props;

        public RpcClient(string in_queue, string out_queue)
        {
            RabbitMQService messagingservice = new RabbitMQService();
            connection = messagingservice.Get_rabbitmq_connection();
            channel = connection.CreateModel();
            consumer = new EventingBasicConsumer(channel);

            //declare in_queue
            messagingservice.Set_up_queue_for_rpc(channel, in_queue);

            if (out_queue == "AUTO")
            {
                replyQueueName = channel.QueueDeclare().QueueName;
            }
            else
            {
                replyQueueName = out_queue;

                //declare out_queue
                messagingservice.Set_up_queue_for_rpc(channel, out_queue);
            }

            props = channel.CreateBasicProperties();
            var correlationId = Guid.NewGuid().ToString();
            props.CorrelationId = correlationId;
            props.ReplyTo = replyQueueName;

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var response = Encoding.UTF8.GetString(body);
                if (ea.BasicProperties.CorrelationId == correlationId)
                {
                    respQueue.Add(response);
                }
            };
        }

        public string Call(string in_queue, string message)
        {
            var messageBytes = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(
                exchange: "",
                routingKey: in_queue,
                basicProperties: props,
                body: messageBytes);

            channel.BasicConsume(
                consumer: consumer,
                queue: replyQueueName,
                autoAck: true);

            return respQueue.Take(); ;
        }

        public void Close()
        {
            connection.Close();
        }
    }
}
