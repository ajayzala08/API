using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATSAPI.Models
{
    public class AppraisalModel
    {
        public decimal paid { get; set; }
        public decimal empcode { get; set; }
        public decimal rmempcode { get; set; }
        public string period { get; set; }
        public DateTime createdon { get; set; }
        public DateTime expiredon { get; set; }
        public string employeestatus { get; set; }
        public DateTime employeeDate { get; set; }
        public string rmstatus { get; set; }
        public DateTime rmDate { get; set; }
    }
}