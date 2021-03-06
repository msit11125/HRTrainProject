﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace HRTrainProject.Web
{
    public static class IIdentityExtionsions
    {
        /// <summary>
        /// 取得Claims Value
        /// </summary>
        /// <param name="claimsIdentity"></param>
        /// <param name="claimType"> System.Security.Claims.ClaimTypes</param>
        /// <returns></returns>
        public static string GetClaimValue(this IIdentity claimsIdentity, string claimType)
        {
            var claim = ((ClaimsIdentity)claimsIdentity).Claims.FirstOrDefault(x => x.Type == claimType);

            return (claim != null) ? claim.Value : string.Empty;
        }
    }
}
