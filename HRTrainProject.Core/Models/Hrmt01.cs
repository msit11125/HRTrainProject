using System;
using System.Collections.Generic;

namespace HRTrainProject.Core.Models
{
    public partial class HRMT01
    {
        public string USER_NO { get; set; }
        public string NAME { get; set; }
        public string SNAME { get; set; }
        public string SEX { get; set; }
        public DateTime? BIRTHDAY { get; set; }
        public byte? USER_STATUS { get; set; }
        public string E_MAIL { get; set; }
        public string TEL_NO { get; set; }
        public string PHONE { get; set; }
        public string IDNO { get; set; }
        public byte? INIT_STATUS { get; set; }
        public string PHOTO { get; set; }
        public string JOB_TITLE { get; set; }
        public string ADDRESS { get; set; }
        public string SERVICE_UNITS { get; set; }
        public DateTime? CHG_DATE { get; set; }
        public string CHG_PERSON { get; set; }
        public string PASSWORD { get; set; }
        public DateTime? EXP_DATE { get; set; }
        public decimal? TALL { get; set; }
        public decimal? WEIGHT { get; set; }
    }
}
