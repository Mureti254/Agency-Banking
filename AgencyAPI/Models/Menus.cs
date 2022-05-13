using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgencyAPI.Models
{
    public class Menus
    {
        public int menuid { get; set; }
        public string name { get; set; }
        public string link { get; set; }
        public string icon { get; set; }
        public int parentmenuid { get; set; }
    }
    public class profile
    {
        public int profileid { get; set; }
    }
    public class MenuAssignment
    {
        public Profile profile { get; set; }
        public List<Menus> menu { get; set; }
    }
}