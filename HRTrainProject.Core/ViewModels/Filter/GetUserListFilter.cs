using System;
using System.Collections.Generic;
using System.Text;

namespace HRTrainProject.Core.ViewModels.Filter
{
    public class GetUserListFilter : BaseFilter
    {
        public string Search_Name { get; set; }
        public string Search_RoleId { get; set; }
    }
}
