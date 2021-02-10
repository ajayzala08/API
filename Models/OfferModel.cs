using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATSAPI.Models
{
    public class OfferModel
    {
        public decimal offerid { get; set; }
        public decimal interviewid { get; set; }
        public decimal resumeid { get; set; }
        public decimal requirementid { get; set; }
        public string name { get; set; }
        public DateTime dob { get; set; }
        public string gender { get; set; }
        public string maritalstatus { get; set; }
        public string currentaddress { get; set; }
        public string permanantaddress { get; set; }
        public string country { get; set; }
        public string email { get; set; }
        public string currentdesignation { get; set; }
        public string offerdesignation { get; set; }
        public string skill { get; set; }
        public string totalexperience { get; set; }
        public string relavantexperience { get; set; }
        public string mobileno { get; set; }
        public string alternativno { get; set; }
        public string panno { get; set; }
        public string clientname { get; set; }
        public string location { get; set; }
        public decimal ctc { get; set; }
        public decimal ectc { get; set; }
        public decimal billrate { get; set; }
        public decimal payrate { get; set; }
        public decimal grossprofit { get; set; }
        public decimal grossprofitmargin { get; set; }
        public DateTime selectiondate { get; set; }
        public DateTime offerdate { get; set; }
        public DateTime joindate { get; set; }
        public DateTime exitdate { get; set; }
        public string emergencyno { get; set; }
        public string bankaccountno { get; set; }
        public string bankname { get; set; }
        public string branch { get; set; }
        public string ifci { get; set; }
        public string addarno { get; set; }
        public string status { get; set; }
        public string note { get; set; }
        public string type { get; set; }
        public string recruitername { get; set; }
        public DateTime recruiterdate { get; set; }
        public string teamleadname { get; set; }
        public DateTime teamleaddate { get; set; }
        public string accountmanager { get; set; }
        public DateTime accountmanagerdate { get; set; }
        public string managerstatus { get; set; }
        public DateTime managerdate { get; set; }
        public string accountstatus { get; set; }
        public DateTime accountdate { get; set; }
        public string adminstatus { get; set; }
        public DateTime admindate { get; set; }
        public int isactive { get; set; }

    }
}