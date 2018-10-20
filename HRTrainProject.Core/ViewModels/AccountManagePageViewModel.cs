using HRTrainProject.Core.ViewModels.Filter;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using X.PagedList;

namespace HRTrainProject.Core.ViewModels
{
    public class AccountManagePageViewModel : GetUserListFilter
    {
        public SelectList RoleSelectList { get; set; }

        public IPagedList<UserEditViewModel> UserList { get; set; }

    }

}
