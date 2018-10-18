using System;
using System.Collections.Generic;
using System.Text;

namespace HRTrainProject.Core.ViewModels
{
    public class RolesViewModel
    {
        public UserRole ROLE_ID { get; set; }
        public string ROLE_NAME { get; set; }
        public int? ROLE_TYPE { get; set; }
        public decimal? ROLE_LEVEL { get; set; }
        public string CHG_PERSON { get; set; }
        public DateTime? CHG_DATE { get; set; }
        public string DEFAULT_YN { get; set; }

        public bool CHECKED { get; set; }
    }
}
