﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATSAPI.Models
{
    public class GraphViewModel
    {
        public string name { get; set; }
        public int submission { get; set; }
        public int interview { get; set; }
        public int offer { get; set; }
        public int start { get; set; }
    }
}