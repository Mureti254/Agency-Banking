using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AgencyAPI.Models
{
    public class UserModel
    {

        public string emailaddress { get; set; }
        public string password { get; set; }
    }
}
