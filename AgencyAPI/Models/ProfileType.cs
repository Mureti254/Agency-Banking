using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgencyAPI.Models
{
    public class ProfileType
    {
        public string name { get; set; }

        public DateTime dateadded { get; set; }

        public DateTime datemodified { get; set; }
        public int profiletypeid { get; set; }
    }
}
