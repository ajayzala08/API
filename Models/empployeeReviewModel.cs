using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATSAPI.Models
{
    public class empployeeReviewModel
    {
        public decimal paid { get; set; }
        public int qualityofworkntaskcompletion { get; set; }
        public int goalsntargetachievement { get; set; }
        public int writtennverbalcommunicationskills { get; set; }
        public int initiativenmotivation { get; set; }
        public int teamworknleadershipskills { get; set; }
        public int abilitytoproblemsolve { get; set; }
        public int attendancenregualrization { get; set; }
        public int total { get; set; }
        public string comment { get; set; }

    }
}