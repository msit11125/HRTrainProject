using HRTrainProject.Core.ViewModels.Filter;
using System;
using System.Collections.Generic;
using System.Text;
using X.PagedList;

namespace HRTrainProject.Core.ViewModels
{
    public class AccountManagePageViewModel : GetUserListFilter
    {
        public IPagedList<UserEditViewModel> UserList { get; set; }

    }

}
