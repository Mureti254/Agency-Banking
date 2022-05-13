using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AgencyAPI.Models
{
    public class EmailSending
    {
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
       ErrorMessage = "Invalid email format")]
        [Required]
        public string RecipientEmail { get; set; }
        public string subject { get; set; }
        public string message { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
       ErrorMessage = "Invalid email format")]
        [Required]
        public string SenderEmail { get; set; }
        public DateTime dateadded { get; set; }
        public DateTime datesent { get; set; }
        public string messagetype { get; set; }
        public int otp { get; set; }
    }
}