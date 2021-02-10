using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATSAPI.Models
{
    public class SalesClientCallModel
    {
        public decimal id { get; set; }
        public DateTime dt { get; set; }
        public string client { get; set; }
        public string poc { get; set; }
        public string agenda { get; set; }
        public string username { get; set; }

    }
}