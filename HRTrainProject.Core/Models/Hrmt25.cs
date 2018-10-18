using System;
using System.Collections.Generic;

namespace HRTrainProject.Core.Models
{
    public partial class Hrmt25
    {
        public string UserNo { get; set; }
        public string RoleId { get; set; }
        public string ChgPerson { get; set; }
        public DateTime? ChgDate { get; set; }
        public string DefaultYn { get; set; }
    }
}
