using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AgencyAPI.Models
{
    public class AgentDTO
    {
        public int superagentid { get; set; }
        public int businesstypeid { get; set; }
        public int ownershiptypeid { get; set; }
        public string businessname { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
        ErrorMessage = "Invalid email format")]
        [Required]
        public string emailaddress { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string phone { get; set; }
        public string address { get; set; }
        public string bizregcert { get; set; }
        public string bizlicense { get; set; }
        public string financialstatement { get; set; }
        public string goodconductcert { get; set; }
        public int agentid { get; set; }
        public int agentholderid { get; set; }
        public Boolean approve { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
        ErrorMessage = "Invalid email format")]
        [Required]
        public string alternative_emailaddress { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string alternative_phonenumber { get; set; }

    }
}
