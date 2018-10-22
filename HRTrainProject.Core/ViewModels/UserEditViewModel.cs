using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRTrainProject.Core.ViewModels
{
    public class UserEditViewModel
    {
        [Required]
        [StringLength(20)]
        [Display(Name = "UserAccount")]
        public string USER_NO { get; set; }
        public string E_MAIL { get; set; }
        public string ADDRESS { get; set; }
        public string PHONE { get; set; }
        [Display(Name = "Name")]
        public string NAME { get; set; }

        public DateTime? CHG_DATE { get; set; }
        public string CHG_PERSON { get; set; }

        public string PHOTO { get; set; }
        public List<RolesViewModel> Roles { get; set; }

        public byte? USER_STATUS { get; set; }
    }
}
