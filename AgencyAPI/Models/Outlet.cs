using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AgencyAPI.Models
{
    public class Outlet
    {
        public string name { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string phone { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
        ErrorMessage = "Invalid email format")]
        [Required]
        public string emailaddress { get; set; }
        public DateTime dateadded { get; set; }
        public DateTime datemodified { get; set; }
        public int agentid { get; set; }
        public int statusid { get; set; }
        public Double latitude { get; set; }
        public Double longitude { get; set; }
        public int agentoutletid { get; set; }
        public int agentoutletholderid { get; set; }
    }
}
