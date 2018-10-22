using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HRTrainProject.Services;
using HRTrainProject.Services.Interfaces;
using HRTrainProject.Core.ViewModels.Filter;
using HRTrainProject.Core;
using System.Globalization;

namespace HRTrainProject.Web.ViewComponents
{
    [ViewComponent(Name = "BulletinListViewComponent")]
    public class BulletinListViewComponent : ViewComponent
    {
        private IBulletinService _bulletinService;

        public BulletinListViewComponent(IBulletinService bulletinService)
        {
            this._bulletinService = bulletinService;
        }

        public async Task<IViewComponentResult> InvokeAsync(
        Bulletin_Class_Type class_type)
        {
            ViewBag.CLASS_TYPE = class_type;

            GetAllBulletinFilter filter = new GetAllBulletinFilter();
            filter.CLASS_TYPE = class_type;
            filter.IsOnDashBoard = true;
            filter.LANGUAGE_ID = CultureInfo.CurrentCulture.Name;
            var items = _bulletinService.GetAllBulletin(filter);

            return View("Default", items);
        }

    }
}
