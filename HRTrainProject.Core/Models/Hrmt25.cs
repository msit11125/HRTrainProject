using System;
using System.Collections.Generic;

namespace HRTrainProject.Core.Models
{
    public partial class HRMT25
    {
        public string USER_NO { get; set; }
        public string ROLE_ID { get; set; }
        public string CHG_PERSON { get; set; }
        public DateTime? CHG_DATE { get; set; }
        public string DEFAULT_YN { get; set; }
    }
}
