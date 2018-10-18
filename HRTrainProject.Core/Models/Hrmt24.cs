using System;
using System.Collections.Generic;

namespace HRTrainProject.Core.Models
{
    public partial class Hrmt24
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string ChgPerson { get; set; }
        public DateTime? ChgDate { get; set; }
        public int? RoleType { get; set; }
        public decimal? RoleLevel { get; set; }
    }
}
