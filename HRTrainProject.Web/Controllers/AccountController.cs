﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HRTrainProject.Services.Interfaces;
using HRTrainProject.Core.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using HRTrainProject.Services.Logics;
using NLog;
using HRTrainProject.Core.ViewModels.Filter;
using Microsoft.AspNetCore.Authorization;
using HRTrainProject.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using HRTrainProject.DAL.Repository;
using System.Threading;

namespace HRTrainProject.Web.Controllers
{
    public class AccountController : Controller
    {
        ILogger logger = LogManager.GetCurrentClassLogger();

        private readonly IUserService _userService;
        private readonly IStringLocalizer<AccountController> _localizer; // <= 多國語系
        private readonly IHostingEnvironment _env; // <= 存取Server路徑等資訊

        string photoDirectory; // Photo目錄

        public AccountController(IUserService userService, IStringLocalizer<AccountController> localizer,
            IHostingEnvironment env)
        {
            this._userService = userService;
            this._localizer = localizer;
            _env = env;

            // 把Photo上傳目錄設為：wwwroot/UploadFolder/UserPhotos (linux 路徑為正斜線)
            photoDirectory = $@"{ _env.WebRootPath}/UploadFolder/UserPhotos";

        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(string returnUrl, LogInModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            UserProfile findUser = _userService.LogIn(model, out string resultCode);
            if (findUser==null)
            {
                ModelState.AddModelError("LoginError", _localizer[resultCode].Value);
                return View(model);
            }

            // 要存的資訊: 看要存字串還是json
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, findUser.USER_NO),
                new Claim(ClaimTypes.NameIdentifier, findUser.USER_NO),
                new Claim(ClaimTypes.MobilePhone, findUser.PHONE ?? "")
            };
            var roles = findUser.Roles
                .Select(r => new Claim(ClaimTypes.Role, LogicCenter.GetEnumName(r.ROLE_ID)));
            
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaims(claims);
            identity.AddClaims(roles);
            // 製作身分驗證Cookie
            var principal = new ClaimsPrincipal(identity);
            if (model.RememberMe)
            {
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                    new AuthenticationProperties { IsPersistent = true });
            }
            else
            {
                // 過期時間
                var timeSpanOffset = DateTimeOffset.UtcNow.AddMinutes(30);

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                    new AuthenticationProperties { IsPersistent = true, ExpiresUtc = timeSpanOffset });
            }

            return Redirect(returnUrl ?? Url.Action("Index", "Home"));
        }

        [HttpGet, HttpPost]
        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }
        [CheckPermissionAttribute]
        [Authorize(Policy = nameof(PolicyGroup.管理者級別))]
        public IActionResult ManagePage(AccountManagePageViewModel vm)
        {
            var userList =  _userService.GetUserList(vm);
            vm.RoleSelectList = _userService.GetRoles()
                .ToSelectList(r => r.ROLE_ID, r => r.ROLE_NAME);

            vm.UserList = userList.ToPageList(vm.Page, vm.PageSize);

            return View(vm);
        }

        [HttpGet]
        [Authorize(Roles = nameof(UserRole.最大管理者))]
        public IActionResult Add()
        {
            UserEditViewModel userDetail = _userService.GetUserDetailAndRolesAll(null);

            return View(userDetail);
        }

        [HttpPost]
        [Authorize(Roles = nameof(UserRole.最大管理者))]
        public async Task<IActionResult> Add(UserEditViewModel model, List<IFormFile> photo)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //File Upload
            if (photo != null && photo.Count > 0)
            {
                string prefix = model.USER_NO + "_";
                FileRepository fileRepo = new FileRepository();
                await fileRepo.UploadPhoto(photo, photoDirectory, prefix);
                model.PHOTO = prefix + photo.FirstOrDefault().FileName;
            }

            string changerNo = HttpContext.User.Identity.GetClaimValue(ClaimTypes.NameIdentifier);

            model.CHG_DATE = DateTime.Now;
            model.CHG_PERSON = changerNo;

            bool state = _userService.AddNewUser(model, out string resultCode);

            TempData["Status"] = state;
            TempData["StatusMessage"] = _localizer[resultCode].Value;

            return RedirectToAction("ManagePage");
        }

        [HttpGet]
        public IActionResult UserDetail(string userNo)
        {
            UserEditViewModel userDetail = _userService.GetUserDetailAndRolesAll(userNo);
            return View(userDetail);
        }

        [HttpGet]
        [Authorize(Roles = nameof(UserRole.最大管理者))]
        public IActionResult Edit(string userNo)
        {
            UserEditViewModel userDetail = _userService.GetUserDetailAndRolesAll(userNo);
            return View(userDetail);
        }

        [HttpPost]
        [Authorize(Roles = nameof(UserRole.最大管理者))]
        public async Task<IActionResult> Edit(UserEditViewModel model, List<IFormFile> photo)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //File Upload
            if (photo != null && photo.Count>0)
            {
                string prefix = model.USER_NO + "_";
                FileRepository fileRepo = new FileRepository();
                await fileRepo.UploadPhoto(photo, photoDirectory, prefix);
                model.PHOTO = prefix + photo.FirstOrDefault().FileName;
            }

            string changerNo = HttpContext.User.Identity.GetClaimValue(ClaimTypes.NameIdentifier);
            model.CHG_DATE = DateTime.Now;
            model.CHG_PERSON = changerNo;

            bool state = _userService.EditUser(model, out string resultCode);

            TempData["Status"] = state;
            TempData["StatusMessage"] = _localizer[resultCode].Value;

            return View(model);
        }

        [HttpGet]
        public IActionResult GetPhotoImage(string photo)
        {
            try
            {
                var image = System.IO.File.OpenRead(System.IO.Path.Combine(photoDirectory, photo));
                return File(image, "image/jpeg");
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}