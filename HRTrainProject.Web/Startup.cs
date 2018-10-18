using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using AutoMapper;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.Cookies;
using HRTrainProject.Core;
using HRTrainProject.DAL;
using HRTrainProject.Services;
using HRTrainProject.Core.Models;
using HRTrainProject.Interfaces;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Security.Principal;

namespace HRTrainProject.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region  多國語系
            services.AddLocalization(s => s.ResourcesPath = "Resources");
            var supportedCultures = new CultureInfo[]
            {
                new CultureInfo("zh-TW"),
                new CultureInfo("en-GB"),
            };
            services.Configure<RequestLocalizationOptions>(s =>
            {
                // Formatting numbers, dates, etc.
                s.SupportedCultures = supportedCultures;
                // UI strings that we have localized.
                s.SupportedUICultures = supportedCultures;
                s.DefaultRequestCulture = new RequestCulture(culture: "zh-TW", uiCulture: "zh-TW");
            });
            services.AddMvc()
                    .AddViewLocalization()
                    .AddDataAnnotationsLocalization(o => {
                        o.DataAnnotationLocalizerProvider = (type, factory) =>
                            factory.Create(typeof(SharedResource));

                    });
            #endregion

            #region Authorize
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // Add authentication services
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options => {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Error/AccessDenied";
            });

            // Policy (權限群組設定)
            services.AddAuthorization(options =>
            {
                options.AddPolicy(nameof(PolicyGroup.管理者級別), 
                    policy => policy.RequireRole(nameof(UserRole.最大管理者), nameof(UserRole.管理者)));
                options.AddPolicy(nameof(PolicyGroup.基層級別),
                    policy => policy.RequireRole(nameof(UserRole.最大管理者), nameof(UserRole.管理者), nameof(UserRole.一般會員)));
            });
            #endregion

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            #region Dependency Injection
            // 注入Config
            var configManager = new ConfigManager();
            Configuration.GetSection("Config").Bind(configManager);
            Mapper.Initialize(cfg => cfg.CreateMap<ConfigManager, ConfigManager>());
            ConfigProvider.ConfigManager = Mapper.Map<ConfigManager>(configManager);

            // 注入Unit、DAL、Service
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUserService, UserService>();

            // 注入DbContext
            services.AddDbContext<HRTrainDbContext>(options =>
                 options.UseSqlServer(Configuration.GetConnectionString("SqlServerConnection")));
            // DbContext不能有建構子才可替換為 AddDbContextPool
            //services.AddDbContextPool<HRTrainDbContext>(options =>
            //    options.UseSqlServer(Configuration.GetConnectionString("SqlServerConnection")));

            // 注入IMapper
            services.AddAutoMapper();
            #endregion

            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); // 注入建構子可加入 IHttpContextAccessor 可取得 HttpContext

#if DEBUG
            this.DEVELOP_SEED();
#endif
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // 加入npm
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "node_modules")),
                RequestPath = new PathString("/vendor")
            });

            // Using 多國語系 
            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);

            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

        }

        [System.ComponentModel.Description("DEVELOP CONTEXT USE")]
        private void DEVELOP_SEED()
        {
            HRTrainDbContext db = new HRTrainDbContext();
            Hrmt01 u = new Hrmt01()
            {
                UserNo = "SuperAdmin",
                EMail = "test@google.com",
                Address = "none",
                Phone = "1234567",
                Name = "Tester",
                ChgDate = DateTime.Now,
                ChgPerson = "System"
            };
            Hrmt24 r = new Hrmt24()
            {
                RoleId = ((int)UserRole.最大管理者).ToString(),
                RoleName = "SuperAdmin",
                ChgPerson = "System"
            };
            Hrmt25 ur = new Hrmt25()
            {
                UserNo = u.UserNo,
                RoleId = r.RoleId,
                DefaultYn = "Y",
                ChgDate = DateTime.Now,
                ChgPerson = "System"
            };
            if (db.Hrmt01.Where(user => user.UserNo == u.UserNo).FirstOrDefault() == null)
            {
                db.Hrmt01.Add(u);
            }
            if (db.Hrmt24.Where(role => role.RoleId == r.RoleId).FirstOrDefault() == null)
            {
                db.Hrmt24.Add(r);
            }
            if (db.Hrmt25.Where(userRole => userRole.UserNo == ur.UserNo && userRole.RoleId == ur.RoleId)
                .FirstOrDefault() == null)
            {
                db.Hrmt25.Add(ur);
            }
            db.SaveChanges();
        }

    }
}
