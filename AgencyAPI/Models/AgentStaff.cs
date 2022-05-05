using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AgencyAPI.Models
{
    public class AgentStaff
    {
        public int agentstaffid { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string surname { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string phone { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
        ErrorMessage = "Invalid email format")]
        [Required]
        public string emailaddress { get; set; }
        public string username { get; set; }
        [DataType(DataType.Password)]
        public string password { get; set; }
        public string otp { get; set; }
        public DateTime otptime { get; set; }
        public DateTime dateadded { get; set; }
        public int profileid { get; set; }
     
        public int agentid { get; set; }
        public int agentoutletid { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
        ErrorMessage = "Invalid email format")]
        [Required]
        public string alternative_emailaddress { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string alternative_phonenumber { get; set; }

    }
}
