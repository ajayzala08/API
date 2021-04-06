using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATSAPI.Controllers
{
    public class DeleteUserModel
    {
        public string ecode { get; set; }
        public DateTime exitdt { get; set; }
        public string attritiontype { get; set; }
        public string reason { get; set; }

    }
}