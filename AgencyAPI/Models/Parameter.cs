using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgencyAPI.Models
{
    public class Parameter
    {
        public string  item_key { get; set; }
        public string item_name { get; set; }
        public Boolean is_encrypted { get; set; }
        public int parameterid { get; set; }
    }
}
