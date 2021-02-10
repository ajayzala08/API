﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATSAPI.Models
{
    public class InterviewViewModel
    {
        public decimal iid { get; set; }
        public string name { get; set; }
        public DateTime idate { get; set; }
        public string itime { get; set; }
        public string by { get; set; }
        public string location { get; set; }
        public string status { get; set; }
        public string recruiter { get; set; }
    }
}