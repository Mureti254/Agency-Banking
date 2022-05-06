using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AgencyAPI.Models
{
    public class PasswordChange
    {
        public string Password { get; set; }
        public DateTime DateTime { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
        ErrorMessage = "Invalid email format")]
        [Required]
        public string Emailaddress { get; set; }
        public int otp { get; set; }
        public string newpassword { get; set; }
        public string confirmpassword { get; set; }
    }
}
