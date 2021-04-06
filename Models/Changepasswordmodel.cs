using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATSAPI.Models
{
    public class Changepasswordmodel
    {
        public string username { get; set; }
        public string oldpwd { get; set; }
        public string newpwd { get; set; }
    }
}