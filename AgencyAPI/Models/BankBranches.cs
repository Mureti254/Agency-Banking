using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgencyAPI.Models
{
    public class BankBranches
    {
        public string name { get; set; }
        public string branchcode { get; set; }
        public int bankbranchid { get; set; }
        public int bankid { get; set; }
    }
}
