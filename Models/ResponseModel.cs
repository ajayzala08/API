using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATSAPI.Models
{
    public class ResponseModel
    {
        public string Message { set; get; }
        public bool Status { set; get; }
        public dynamic Data { set; get; }
    }
}