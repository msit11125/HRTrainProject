using System;
using System.Collections.Generic;

namespace HRTrainProject.Core.Models
{
    public partial class Hrmt01
    {
        public string UserNo { get; set; }
        public string Name { get; set; }
        public string Sname { get; set; }
        public string Sex { get; set; }
        public DateTime? Birthday { get; set; }
        public byte? UserStatus { get; set; }
        public string EMail { get; set; }
        public string TelNo { get; set; }
        public string Phone { get; set; }
        public string Idno { get; set; }
        public byte? InitStatus { get; set; }
        public string Photo { get; set; }
        public string JobTitle { get; set; }
        public string Address { get; set; }
        public string ServiceUnits { get; set; }
        public DateTime? ChgDate { get; set; }
        public string ChgPerson { get; set; }
        public string Password { get; set; }
        public DateTime? ExpDate { get; set; }
        public decimal? Tall { get; set; }
        public decimal? Weight { get; set; }
    }
}
