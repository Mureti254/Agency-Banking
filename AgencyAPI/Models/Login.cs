using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AgencyAPI.Models
{
    public class Login
    {
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
        ErrorMessage = "Invalid email format")]
        [Required]
        public string emailaddress { get; set; }

        [DataType(DataType.Password)]
        public string password { get; set; }
        public string otp { get; set; }
        public DateTime otpdate { get; set; }
        public DateTime otptime { get; set; }

    }
}
