using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATSAPI.Models
{
    public class AttendanceDataViewModel
    {
        public String intime { get; set; }
        public String outtime { get; set; }
        public String totaltime { get; set; }
        public String firstHalf { get; set; }
        public String secondHalf { get; set; }
        public String attendanceDate { get; set; }
        public String shiftName { get; set; }
    }
}