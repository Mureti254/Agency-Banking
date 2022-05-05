using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgencyAPI.Models
{
    public class Device
    {
        public string serialnumber { get; set; }
        public string device_name { get; set; }
        public string device_model { get; set; }
        public string firmware_version { get; set; }

        public int deviceid { get; set; }
        public int deviceholderid { get; set; }
    }
}
