using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATSAPI.Models
{
    public class ViewRevenueReportModel
    {
        public string Name { get; set; }
        public string Client { get; set; }
        public string Location { get; set; }
        public string Skill { get; set; }
        public string Type { get; set; }
        public double CTC { get; set; }
        public double BR { get; set; }
        public double PR { get; set; }
        public double GP { get; set; }
        public double GPM { get; set; }
        public string Recruiter { get; set; }
    }
}