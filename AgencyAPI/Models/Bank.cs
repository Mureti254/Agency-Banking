using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgencyAPI.Models
{
    public class Bank
    {
        public string name { get; set; }
        public string swiftcode { get; set; }
        public string bankcode { get; set; }
        public int bankid { get; set; }
        public string banklogo { get; set; }

    }
}
