using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATSAPI.Models
{
    public class ViewOfferModel
    {
        public decimal oid { get; set; }
        public string name { get; set; }
        public string client { get; set; }
        public string skill { get; set; }
        public string location { get; set; }
        public string type { get; set; }
        public string status { get; set; }
        public string recruiter { get; set; }
        public string offdate { get; set; }
    }
}