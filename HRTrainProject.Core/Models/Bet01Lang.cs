using System;
using System.Collections.Generic;

namespace HRTrainProject.Core.Models
{
    public partial class Bet01Lang
    {
        public string ClassType { get; set; }
        public string LanguageId { get; set; }
        public string ClassName { get; set; }
        public string CrePerson { get; set; }
        public DateTime? CreDate { get; set; }
        public string ChgPerson { get; set; }
        public DateTime? ChgDate { get; set; }
        public string Memo { get; set; }
    }
}
