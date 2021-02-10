using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATSAPI.Models
{
    public class AttendanceModel
    {
        public decimal aid { get; set; }
        public string aname { get; set; }
        public string intime { get; set; }
        public string inip { get; set; }
        public string outtime { get; set; }
        public string outip { get; set; }
        public string atime { get; set; }
       
    }
}