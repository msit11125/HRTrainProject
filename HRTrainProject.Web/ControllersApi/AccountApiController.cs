using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HRTrainProject.Core.ViewModels;
using HRTrainProject.Services.Interfaces;
using HRTrainProject.Services.Logics;
using HRTrainProject.Web.Controllers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using NLog;

namespace HRTrainProject.Web.ControllersApi
{
    [Route("api/account")]
    public class AccountApiController : Controller
    {
        ILogger logger = LogManager.GetCurrentClassLogger();

        private readonly IUserService _userService;
        private readonly IStringLocalizer<AccountController> _localizer; // <= 多國語系
        public IConfiguration Configuration { get; } // <= 設定檔案


        public AccountApiController(IUserService userService, IStringLocalizer<AccountController> localizer,
             IConfiguration configuration)
        {
            this._userService = userService;
            this._localizer = localizer;
            this.Configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login(string returnUrl, LogInModel model)
        {
            UserProfile findUser = _userService.LogIn(model, out string resultCode);
            if (findUser == null)
            {
                return Ok(model);
            }

            // 要存的資訊
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, findUser.USER_NO),
                new Claim(ClaimTypes.NameIdentifier, findUser.USER_NO),
                new Claim(ClaimTypes.MobilePhone, findUser.PHONE ?? "")
            };
            var roles = findUser.Roles
                .Select(r => new Claim(ClaimTypes.Role, LogicCenter.GetEnumName(r.ROLE_ID)));
            claims.AddRange(roles);

            // Json Web Token 登入
            var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken
               (
                   issuer: Configuration["Tokens:ValidIssuer"],
                   audience: Configuration["Tokens:ValidAudience"],
                   claims: claims,
                   expires: DateTime.UtcNow.AddHours(1), /* 過期時間 */
                   signingCredentials: new SigningCredentials(new SymmetricSecurityKey
                               ( System.Text.Encoding.UTF8.GetBytes(Configuration["Tokens:IssuerSigningKey"])),
                                 SecurityAlgorithms.HmacSha256)
               );

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(
                new {
                    user = findUser,
                    token = tokenString
                });
        }
        
        // 同時讓Cookies認證和Token認證都可以通過
        // This API will accept both cookie authentication and JWT bearer authentication.
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpGet("getuserid")]
        public string GetUserID()
        {
            return this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

    }
}