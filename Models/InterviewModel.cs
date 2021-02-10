using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATSAPI.Models
{
    public class InterviewModel
    {
        public decimal id { get; set; }
        public decimal resumeid { get; set; }
        public decimal requirementid { get; set; }
        public DateTime interviewdate { get; set; }
        public string interviewtime { get; set; }
        public string interviewby { get; set; }
        public string interviewlocation { get; set; }
        public string interviewnote { get; set; }
        public string status { get; set; }
        public string username { get; set; }
    }
}