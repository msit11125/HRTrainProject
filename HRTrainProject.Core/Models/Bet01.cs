﻿using System;
using System.Collections.Generic;

namespace HRTrainProject.Core.Models
{
    public partial class BET01
    {
        public string CLASS_TYPE { get; set; }
        public int? ORDER_BY { get; set; }
        public int? SHOW_COUNT { get; set; }
        public string CRE_PERSON { get; set; }
        public DateTime? CRE_DATE { get; set; }
        public string CHG_PERSON { get; set; }
        public DateTime? CHG_DATE { get; set; }
        public string MEMO { get; set; }
    }
}
