using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgencyAPI.Models
{
    public class Menu
    {
        public int menuid { get; set; }
    }
        public class profile
    {
        public int profileid { get; set; }
    }
    public class menuassignment
    {
        public profile profile { get; set; }
        public List<Menu> menus { get; set; }
    }
}
