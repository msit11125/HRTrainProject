using HRTrainProject.Web.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HRTrainProject.Web
{
    public class CheckPermissionAttribute: ActionFilterAttribute
    {
        /// <summary>
        /// 要檢查的程式代碼，未傳入預設Controller Name
        /// </summary>
        public string CheckBRE_NO { get; set; }

        /// <summary>
        /// 要檢查的動作代碼，未傳入預設ACTION Name
        /// </summary>
        public string CheckACTION_ID { get; set; }

        /// <summary>
        /// 無權限時 回傳類型 未傳入預設 轉至 RedirectToRouteResult
        /// </summary>
        public string ResultType { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext httpContext = filterContext.HttpContext;

            Controller control = filterContext.Controller as Controller;
            string BRE_NO = CheckBRE_NO != null ? CheckBRE_NO : control.RouteData.Values["Controller"].ToString();
            string ACTION_ID = CheckACTION_ID != null ? CheckACTION_ID : control.RouteData.Values["action"].ToString();
            
            if (!CheckDbPermissson(httpContext.User.Identity.GetClaimValue(ClaimTypes.NameIdentifier)))
            {
                if (ClientHelpers.IsAjaxRequest(httpContext.Request) 
                    || ClientHelpers.IsApiRequest(httpContext.Request))
                {

                    var data = "{ \"Success\" : \"false\" , \"Error\" : \"您無權異動資料\" }";
                    var JsonResult = new JsonResult(data);

                    filterContext.Result = JsonResult;
                }
                else
                {
                    filterContext.Result = new ForbidResult();
                }
            }

            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// 自訂權限檢查 ...
        /// </summary>
        /// <returns></returns>
        private bool CheckDbPermissson(string userno)
        {
            // Check Db CustomPermisson
            return true;
        }
    }
}
