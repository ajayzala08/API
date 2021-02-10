using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATSAPI.Models
{
    public class DailyTargetModel
    {
        public string username { get; set; }
        public int target { get; set; }
        public int achived { get; set; }
        public int closure { get; set; }

    }
}