using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AgencyAPI.Models
{
    public class Complaint
    {
        public int outletid { get; set; }
        public DateTime dateofvisit { get; set; }
        public DateTime datereported { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string customerphonenumber { get; set; }
        public Boolean resolved { get; set; }
        public string remarks { get; set; }
        public string complaintdescription { get; set; }
        public int complaintid { get; set; }

    }
}
