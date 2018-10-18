using HRTrainProject.Core.ViewModels;
using HRTrainProject.Core.ViewModels.Filter;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRTrainProject.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// 登入系統
        /// </summary>
        /// <param name="user"></param>
        /// <param name="resultCode">多國語系代碼</param>
        /// <returns></returns>
        UserProfile LogIn(LogInModel user, out string resultCode);

        /// <summary>
        /// 取得 User 擁有的角色權限
        /// </summary>
        /// <param name="userNo"></param>
        /// <returns></returns>
        List<UserRoles> GetUserRoles(string userNo);

        /// <summary>
        /// 取得 User 列表
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        List<UserEditViewModel> GetUserList(GetUserListFilter filter);

        /// <summary>
        /// 取得User詳細資料
        /// </summary>
        /// <param name="userNo"></param>
        /// <returns></returns>
        UserEditViewModel GetUserDetail(string userNo);

        /// <summary>
        /// 新增User資料
        /// </summary>
        /// <param name="user"></param>
        /// <param name="resultCode">多國代碼</param>
        /// <param name="changer">異動者</param>
        /// <returns></returns>
        bool AddNewUser(UserEditViewModel user, out string resultCode, string changer = "");

        /// <summary>
        /// 修改User資料
        /// </summary>
        /// <param name="user"></param>
        /// <param name="resultCode">多國代碼</param>
        /// <param name="changer">異動者</param>
        /// <returns></returns>
        bool EditUser(UserEditViewModel user, out string resultCode, string changer = "");

    }
}
