using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ATSAPI.Models
{
    public class AppraisalViewModel
    {
        
        public decimal paid { get; set; }
        public decimal empcode { get; set; }
        public string empname { get; set; }
        public decimal rmempcode { get; set; }
        public string rmname { get; set; }
        public string period { get; set; }
        public DateTime createdOn { get; set; }
        public DateTime expiredOn { get; set; }
        

    }
}