using System;
using System.Collections.Generic;

namespace HRTrainProject.Core.Models
{
    public partial class Bet02Lang
    {
        public string BulletId { get; set; }
        public string LanguageId { get; set; }
        public string Subject { get; set; }
        public string ContentTxt { get; set; }
        public string CrePerson { get; set; }
        public DateTime? CreDate { get; set; }
        public string ChgPerson { get; set; }
        public DateTime? ChgDate { get; set; }

        public Bet02 Bullet { get; set; }
    }
}
