using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRTrainProject.Core.ViewModels
{
    public class UserProfile
    {
        public string UserNo { get; set; }
        public string EMail { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }

        public string Photo { get; set; }
        public List<UserRoles> Roles { get; set; }
    }

    public class UserRoles
    {
        public UserRole RoleId { get; set; }
        public string RoleName { get; set; }
        public int? RoleType { get; set; }
        public decimal? RoleLevel { get; set; }
    }
}
