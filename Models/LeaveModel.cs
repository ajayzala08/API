using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATSAPI.Models
{
    public class LeaveModel
    {
        public decimal id { get; set; }
        public string type { get; set; }
        public float noofdays { get; set; }
        public string inwords { get; set; }
        public string reason { get; set; }
        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }
        public string createby { get; set; }
        public DateTime createon { get; set; }
        public string leadstatus { get; set; }
        public DateTime leaddate { get; set; }
        public string managerstatus { get; set; }
        public DateTime managerdate { get; set; }
        public string adminstatus { get; set; }
        public DateTime admindate { get; set; }



    }
}