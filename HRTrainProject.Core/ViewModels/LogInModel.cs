using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRTrainProject.Core.ViewModels
{
    public class LogInModel
    {
        [Display(Name = "Username")]
        [Required(ErrorMessage = "{0} is required")]
        public string USER_NO { get; set; }
        [Display(Name = "Password")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
