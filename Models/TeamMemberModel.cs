using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATSAPI.Models
{
    public class TeamMemberModel
    {
        public decimal tid { get; set; }
        public string teamlead { get; set; }
        public string teammember { get; set; }
        public string createdby { get; set; }
    }
}