using System;
using System.Collections.Generic;

namespace HRTrainProject.Core.Models
{
    public partial class Bet02
    {
        public Bet02()
        {
            Bet02Lang = new HashSet<Bet02Lang>();
        }

        public string BulletId { get; set; }
        public string ClassType { get; set; }
        public string Ispublish { get; set; }
        public DateTime? SDate { get; set; }
        public DateTime? EDate { get; set; }
        public string TopYn { get; set; }
        public string CrePerson { get; set; }
        public DateTime? CreDate { get; set; }
        public string ChgPerson { get; set; }
        public DateTime? ChgDate { get; set; }
        public string Memo { get; set; }

        public ICollection<Bet02Lang> Bet02Lang { get; set; }
    }
}
