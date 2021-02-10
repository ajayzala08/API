using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATSAPI.Models
{
    public class SalesMonthlyReportViewModel
    {
        public decimal id { get; set; }
        public string months { get; set; }
        public string years { get; set; }
        public string client { get; set; }
        public int position { get; set; }
        public int business { get; set; }
        public int submission { get; set; }
        public int intreceived { get; set; }
        public int feedbackpending { get; set; }
        public int noshow { get; set; }
        public int offer { get; set; }
        public int bd { get; set; }
        public int join { get; set; }
        public int passthrough { get; set; }
        public int bulkdeal { get; set; }
        public int poextend { get; set; }
        public int attrition { get; set; }
        public decimal totrevenue { get; set; }
        public string remark { get; set; }
        public string name { get; set; }
    }
}