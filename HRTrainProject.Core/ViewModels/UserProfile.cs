using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRTrainProject.Core.ViewModels
{
    public class UserProfile
    {
        public string USER_NO { get; set; }
        public string E_MAIL { get; set; }
        public string ADDRESS { get; set; }
        public string PHONE { get; set; }
        [Display(Name = "Name")]
        public string NAME { get; set; }

        public string PHOTO { get; set; }
        public List<UserRoles> Roles { get; set; }
    }

    public class UserRoles
    {
        public UserRole ROLE_ID { get; set; }
        public string ROLE_NAME { get; set; }
        public int? ROLE_TYPE { get; set; }
        public decimal? ROLE_LEVEL { get; set; }
    }
}
