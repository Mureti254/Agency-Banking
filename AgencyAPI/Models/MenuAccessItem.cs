using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgencyAPI.Models
{
    public class MenuAccessItem
    {
        public string name { get; set; }
        public string link { get; set; }
        public int profileid { get; set; }
        public int parentmenuid { get; set; }
        public int menuid { get; set; }
        public int statusid { get; set; }
        
    }
}