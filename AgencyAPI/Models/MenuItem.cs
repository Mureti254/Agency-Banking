using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgencyAPI.Models
{
    public class MenuItem
    {
        public string name { get; set; }
        public string link { get; set; }
        public string icon { get; set; }
        public int parentmenuid { get; set; }
        public int statusid { get; set; }


    }
}
