using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgencyAPI.Models
{
    public class Profile
    {
        public string name { get; set; }

        public DateTime dateadded { get; set; }

        public DateTime datemodified { get; set; }

        public int profileid { get; set; }
        public int profiletype_id { get; set; }

    }
}
