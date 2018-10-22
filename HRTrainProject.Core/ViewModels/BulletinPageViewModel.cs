using HRTrainProject.Core.ViewModels.Filter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using X.PagedList;

namespace HRTrainProject.Core.ViewModels
{
    public class BulletinPageViewModel : GetAllBulletinFilter
    {
        public IPagedList<BulletinEditViewModel> BulletinList { get; set; }
    }
}
