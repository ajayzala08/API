using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATSAPI.Models
{
    public class ResumeUploadModel
    {
        public decimal jid { get; set; }
        public string username { get; set; }
        public HttpPostedFile httpPostedFile { get; set; }
    }
}