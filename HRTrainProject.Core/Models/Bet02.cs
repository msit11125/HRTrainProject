using System;
using System.Collections.Generic;

namespace HRTrainProject.Core.Models
{
    public partial class BET02
    {
        public BET02()
        {
            BET02_LANG = new HashSet<BET02_LANG>();
        }

        public string BULLET_ID { get; set; }
        public string CLASS_TYPE { get; set; }
        public string ISPUBLISH { get; set; }
        public DateTime? S_DATE { get; set; }
        public DateTime? E_DATE { get; set; }
        public string TOP_YN { get; set; }
        public string CRE_PERSON { get; set; }
        public DateTime? CRE_DATE { get; set; }
        public string CHG_PERSON { get; set; }
        public DateTime? CHG_DATE { get; set; }
        public string MEMO { get; set; }

        public ICollection<BET02_LANG> BET02_LANG { get; set; }
    }
}
