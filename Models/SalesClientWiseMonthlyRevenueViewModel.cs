using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATSAPI.Models
{
    public class SalesClientWiseMonthlyRevenueViewModel
    {
        public decimal id { get; set; }
        public string salesname { get; set; }
        public string month { get; set; }
        public string year { get; set; }
        public string clientname { get; set; }
        public int currenthc { get; set; }
        public decimal totalgp { get; set; }
        public decimal averagegp { get; set; }
        public int start { get; set; }
        public int attrition { get; set; }
        public int bd { get; set; }
        public int actualstart { get; set; }
        public decimal nettotalgp { get; set; }
        public decimal nettotalgpadded { get; set; }
        public string typeofemployment { get; set; }
    }
}