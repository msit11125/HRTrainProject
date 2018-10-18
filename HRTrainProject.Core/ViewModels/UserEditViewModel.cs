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
        public string UserNo { get; set; }
        public string EMail { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }

        public DateTime? ChgDate { get; set; }

        public string Photo { get; set; }
        public List<RolesViewModel> Roles { get; set; }
    }
}
