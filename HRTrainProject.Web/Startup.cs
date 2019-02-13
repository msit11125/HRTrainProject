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
using HRTrainProject.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using HRTrainProject.Web.Helpers;
using HRTrainProject.DAL.Interfaces;

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
            #region Authorize
            
            // Add authentication services
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {  
                /* Cookies Authorize */
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Error/AccessDenied";
                options.Events.OnRedirectToLogin = ctx =>
                {
                    if (!(ClientHelpers.IsAjaxRequest(ctx.Request) || ClientHelpers.IsApiRequest(ctx.Request)))
                    {
                        ctx.Response.Redirect(ctx.RedirectUri);
                        return Task.CompletedTask;
                    }
                    ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return ctx.Response.WriteAsync("Unauthorized");
                };
                options.Events.OnRedirectToAccessDenied = ctx =>
                {
                    if (!(ClientHelpers.IsAjaxRequest(ctx.Request) || ClientHelpers.IsApiRequest(ctx.Request)))
                    {
                        ctx.Response.Redirect(options.AccessDeniedPath);
                        return Task.CompletedTask;
                    }
                    ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return ctx.Response.WriteAsync("Your member's authority is not enough.");
                };
            })
            .AddJwtBearer(options =>
            {
                /* 驗證 Json Web Token 對應 */
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Tokens:ValidIssuer"],
                    ValidAudience = Configuration["Tokens:ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:IssuerSigningKey"])),
                    RequireExpirationTime = true,
                };
                options.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = context =>
                    {
                        context.NoResult();
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        Console.WriteLine("OnTokenValidated: " +
                            context.SecurityToken);
                        return Task.CompletedTask;
                    }
                };
            });

            // Policy (權限群組設定)
            services.AddAuthorization(options =>
            {
                options.AddPolicy(nameof(PolicyGroup.管理者級別), 
                    policy => policy.RequireRole(nameof(UserRole.最大管理者), nameof(UserRole.管理者)));
                options.AddPolicy(nameof(PolicyGroup.基層級別),
                    policy => policy.RequireRole(nameof(UserRole.一般會員)));
            });
            #endregion

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
            services.AddScoped<IBulletinService, BulletinService>();

            // 注入DbContext
            services.AddDbContext<HRTrainDbContext>(options =>
                 options.UseSqlServer(Configuration.GetConnectionString("SqlServerConnection")));

            // 注入IMapper
            services.AddAutoMapper();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); // 注入建構子可加入 IHttpContextAccessor 可取得 HttpContext

            #endregion

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

            services.AddMvc()
                .AddJsonOptions(options =>
                {
                                /* 去除循環參考 */
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

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

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "api",
                    template: "api/{controller}");

            });
        }

        #region DEBUG SEED
        [System.ComponentModel.Description("DEVELOP CONTEXT USE")]
        private void DEVELOP_SEED()
        {
            var optionsBuilder = new DbContextOptionsBuilder<HRTrainDbContext>();
            optionsBuilder.UseSqlServer(Configuration.GetConnectionString("SqlServerConnection"));

            HRTrainDbContext db = new HRTrainDbContext(optionsBuilder.Options);

            HRMT01 u = new HRMT01()
            {
                USER_NO = "SuperAdmin",
                E_MAIL = "test@google.com",
                ADDRESS = "none",
                PASSWORD = "1234567",
                NAME = "Tester",
                CHG_DATE = DateTime.Now,
                CHG_PERSON = "System"
            };
            HRMT24 r = new HRMT24()
            {
                ROLE_ID = ((int)UserRole.最大管理者).ToString(),
                ROLE_NAME = "SuperAdmin",
                CHG_PERSON = "System"
            };
            HRMT25 ur = new HRMT25()
            {
                USER_NO = u.USER_NO,
                ROLE_ID = r.ROLE_ID,
                DEFAULT_YN = "Y",
                CHG_DATE = DateTime.Now,
                CHG_PERSON = "System"
            };
            if (db.HRMT01.Where(user => user.USER_NO == u.USER_NO).FirstOrDefault() == null)
            {
                db.HRMT01.Add(u);
            }
            if (db.HRMT24.Where(role => role.ROLE_ID == r.ROLE_ID).FirstOrDefault() == null)
            {
                db.HRMT24.Add(r);
            }
            if (db.HRMT25.Where(userRole => userRole.USER_NO == ur.USER_NO && userRole.ROLE_ID == ur.ROLE_ID)
                .FirstOrDefault() == null)
            {
                db.HRMT25.Add(ur);
            }
            db.SaveChanges();
        }
        #endregion

    }
}
