using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATSAPI.Models
{
    public class SalesClientModel
    {
        public decimal id { get; set; }
        public decimal uid { get; set; }
        public decimal cid { get; set; }
        public string username { get; set; }
        public DateTime createdatetime { get; set; }
    }
}