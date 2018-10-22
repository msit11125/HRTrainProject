using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HRTrainProject.Core.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using HRTrainProject.Core.ViewModels;
using HRTrainProject.Services.Logics;
using HRTrainProject.Core;
using HRTrainProject.Core.ViewModels.Filter;
using System.Globalization;
using HRTrainProject.Services.Interfaces;

namespace HRTrainProject.Web.Controllers
{
    public class HomeController : Controller
    {

        private readonly IStringLocalizer<HomeController> _localizer;
        private readonly IBulletinService _bulletinService;

        public HomeController(IStringLocalizer<HomeController> localizer, IBulletinService _bulletinService)
        {
            this._localizer = localizer;
            this._bulletinService = _bulletinService;
        }

        public IActionResult Index()
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            UserProfile u = new UserProfile();
            u.NAME = claimsIdentity.Name;
            u.PHONE = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.MobilePhone).FirstOrDefault()?.Value;

            #region Bulletin View Component
            ViewBag.BulletinClassType = 1;
            #endregion

            return View(u);
        }


        public IActionResult About()
        {
            return View();
        }

        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }
    }
}
