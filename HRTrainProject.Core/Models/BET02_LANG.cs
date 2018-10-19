using System;
using System.Collections.Generic;

namespace HRTrainProject.Core.Models
{
    public partial class BET02_LANG
    {
        public string BULLET_ID { get; set; }
        public string LANGUAGE_ID { get; set; }
        public string SUBJECT { get; set; }
        public string CONTENT_TXT { get; set; }
        public string CRE_PERSON { get; set; }
        public DateTime? CRE_DATE { get; set; }
        public string CHG_PERSON { get; set; }
        public DateTime? CHG_DATE { get; set; }

        public BET02 BULLET_ { get; set; }
    }
}
