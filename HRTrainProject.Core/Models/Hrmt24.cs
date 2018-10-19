using System;
using System.Collections.Generic;

namespace HRTrainProject.Core.Models
{
    public partial class HRMT24
    {
        public string ROLE_ID { get; set; }
        public string ROLE_NAME { get; set; }
        public string CHG_PERSON { get; set; }
        public DateTime? CHG_DATE { get; set; }
        public int? ROLE_TYPE { get; set; }
        public decimal? ROLE_LEVEL { get; set; }
    }
}
