using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using HRTrainProject.Core;
using HRTrainProject.Core.ViewModels;
using HRTrainProject.Core.ViewModels.Filter;
using HRTrainProject.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using NLog;

namespace HRTrainProject.Web.Controllers
{
    public class BulletinController : Controller
    {
        ILogger logger = LogManager.GetCurrentClassLogger();

        private readonly IStringLocalizer<BulletinController> _localizer; // <= 多國語系
        private readonly IBulletinService _bulletinService;


        public BulletinController(IBulletinService _bulletinService, IStringLocalizer<BulletinController> localizer)
        {
            this._bulletinService = _bulletinService;
            this._localizer = localizer;

        }

        public IActionResult BulletinPage(BulletinPageViewModel vm)
        {
            vm.CLASS_TYPE = vm.CLASS_TYPE == 0 ? Bulletin_Class_Type.系統公告 : vm.CLASS_TYPE;
            vm.LANGUAGE_ID = CultureInfo.CurrentCulture.Name;
            List<BulletinEditViewModel> bulletins = _bulletinService.GetAllBulletin(vm);
            vm.BulletinList = bulletins.ToPageList(vm.Page, vm.PageSize);

            // 權限Button檢查 ...
            if(HttpContext.User.IsInRole(nameof(UserRole.管理者)) || HttpContext.User.IsInRole(nameof(UserRole.最大管理者)))
            {
                vm.BtnPermissions.Add(BtnPermission.Insert);
                vm.BtnPermissions.Add(BtnPermission.Edit);
                vm.BtnPermissions.Add(BtnPermission.Delete);
            }

            return View(vm);
        }

        public IActionResult _BulletinContentPartial(BulletinPageViewModel vm)
        {
            return PartialView(vm);
        }

        public IActionResult Detail(string bullet_id)
        {
            string language_id = CultureInfo.CurrentCulture.Name;

            BulletinEditViewModel bulletin = _bulletinService.GetBulletinDetail(bullet_id, language_id, out string resultCode);
            if(bulletin == null)
            {
                string errorCode = _localizer[resultCode].Value;
            }

            return View(bulletin);
        }
    }
}