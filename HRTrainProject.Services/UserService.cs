using AutoMapper;
using HRTrainProject.Core.Models;
using HRTrainProject.Interfaces;
using HRTrainProject.Core.ViewModels;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRTrainProject.Services.Logics;
using HRTrainProject.Services.Extensions;
using HRTrainProject.Core;
using HRTrainProject.Core.ViewModels.Filter;
using Microsoft.EntityFrameworkCore;
using Dapper;

namespace HRTrainProject.Services
{
    public class UserService : IUserService
    {
        ILogger logger = LogManager.GetCurrentClassLogger();

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Hrmt01> _userRepo;
        private readonly IGenericRepository<Hrmt24> _roleRepo;
       
        public UserService(IUnitOfWork unitOfWork, IMapper mapper,
            IGenericRepository<Hrmt01> userDAL, IGenericRepository<Hrmt24> roleDAL)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
            this._userRepo = userDAL;
            this._roleRepo = roleDAL;
        }

        public UserProfile LogIn(LogInModel user, out string resultCode)
        {
            UserProfile userProfile = null;
            var o_user = _userRepo.Get(u => u.UserNo == user.UserNo).FirstOrDefault();
            if (o_user == null)
            {
                resultCode = "notFindUser";
                return null;
            }

            // 檢查密碼
            if (!true)
            {
                resultCode = "passwordError";
                return null;
            }

            userProfile = _mapper.Map<Hrmt01, UserProfile>(o_user);
            userProfile.Roles = this.GetUserRoles(user.UserNo);

            resultCode = "loginSuccess";
            logger.Trace($@"{user.UserNo} 登入系統成功。");

            return userProfile;
        }

        public List<UserRoles> GetUserRoles(string userNo)
        {
            var query_userRole = _unitOfWork.Db.Hrmt25
                .Join(_unitOfWork.Db.Hrmt24, uur => uur.RoleId, r => r.RoleId, 
                (uur, r) => new { uur.UserNo, uur.RoleId, r.RoleName, r.RoleType, r.RoleLevel })
                .Where(w=>w.UserNo == userNo)
                .AsEnumerable()
                .Select(s => new UserRoles
                {
                    RoleId = s.RoleId.ToEnum<UserRole>(),
                    RoleName = s.RoleName,
                    RoleType = s.RoleType,
                    RoleLevel = s.RoleLevel
                });

            return query_userRole.ToList();
        }

        public List<UserEditViewModel> GetUserList(GetUserListFilter filter)
        {
            var queryUser = _userRepo.Get();

            // 篩選 ...
            if (filter.QueryString != null)
            {
                queryUser = queryUser.Where(u => u.UserNo.Contains(filter.QueryString));
            }
            if(filter.SearchName != null)
            {
                queryUser = queryUser.Where(u => u.Name.Contains(filter.SearchName));
            }

            // 排序 ...
            if (filter.OrderColumn == null) // 預設排序
            {
                filter.OrderColumn = "UserNo";
                filter.SortBy = SortBy.ASC;
            }
            var propertyInfo = typeof(Hrmt01).GetProperty(filter.OrderColumn);
            if (filter.SortBy == SortBy.ASC)
            {
                queryUser = queryUser.OrderBy(x => propertyInfo.GetValue(x, null));
            }
            else
            {
                queryUser = queryUser.OrderByDescending(x => propertyInfo.GetValue(x, null));
            }

            var queryUserList = queryUser.ToList();
            var userProfiles = _mapper.Map<List<Hrmt01>, List<UserEditViewModel>>(queryUserList);

            return userProfiles;
        }

        public UserEditViewModel GetUserDetail(string userNo)
        {
            var ouser = _userRepo.Get(u => u.UserNo == userNo).FirstOrDefault()?? new Hrmt01();
            var userDetail = _mapper.Map<Hrmt01, UserEditViewModel>(ouser);
           
            var query_userRole =  _unitOfWork.Db.Database.GetDbConnection().Query<RolesViewModel>
                (@"select r.ROLE_ID, r.ROLE_NAME, r.ROLE_LEVEL, r.ROLE_TYPE, ur.CHG_DATE, ur.CHG_PERSON, ur.DEFAULT_YN,
                           CAST(
                           case when  ur.USER_NO is null then 0 else 1 end 
                           AS BIT) as CHECKED
                             from hrmt24 r
                           left join (select * from hrmt25 where USER_NO =@USER_NO) ur 
                           on r.ROLE_ID = ur.ROLE_ID "
                , new
                {
                    USER_NO = userNo
                }).ToList();
            userDetail.Roles = query_userRole;

            return userDetail;
        }

        public bool AddNewUser(UserEditViewModel user, out string resultCode, string changer = "")
        {
            try
            {
                var n_user = _mapper.Map<UserEditViewModel, Hrmt01>(user);

                _unitOfWork.Db.Hrmt01.Add(n_user);
                _unitOfWork.Db.Hrmt25.AddRange(
                    user.Roles.Where(r => r.CHECKED == true).Select(ur => new Hrmt25()
                    {
                        RoleId = ((int)ur.ROLE_ID).ToString(),
                        UserNo = user.UserNo,
                        ChgDate = DateTime.Now,
                        ChgPerson = changer,
                        DefaultYn = ""
                    }));

                _unitOfWork.Db.SaveChanges();

                resultCode = "add user success";
            }
            catch (Exception ex)
            {
                resultCode = ex.Message;
                return false;
            }

            return true;
        }

        public bool EditUser(UserEditViewModel user, out string resultCode, string changer = "")
        {
            // 修改User角色權限Func
            Action<string, IEnumerable<RolesViewModel>> editUserRoles = (userNo, userRoles) =>
            {
                userRoles = userRoles.OrderBy(r => r.ROLE_ID).ToList();
                var o_user_role = _unitOfWork.Db.Hrmt25.Where(u => u.UserNo == userNo).OrderBy(r => r.RoleId).ToList();
                if (!LogicCenter.IsTheSame(
                    userRoles,
                    o_user_role,
                    (ur, our) => ((int)ur.ROLE_ID).ToString() == our.RoleId))
                {
                    _unitOfWork.Db.Hrmt25.RemoveRange(o_user_role);
                    _unitOfWork.Db.Hrmt25.AddRange(userRoles.Select(ur => new Hrmt25()
                    {
                        RoleId = ((int)ur.ROLE_ID).ToString(),
                        UserNo = userNo,
                        ChgDate = DateTime.Now,
                        ChgPerson = changer,
                        DefaultYn = ""
                    }));
                }
            };

            try
            {
                // update user infos
                var o_user = _userRepo.Get(u => u.UserNo == user.UserNo).FirstOrDefault();
                if (o_user == null)
                {
                    resultCode = "notFindUser";
                    return false;
                }
                    
                if (string.IsNullOrEmpty(user.Photo)) // 圖片沒換就不更新，一樣使用舊圖
                    user.Photo = o_user.Photo;

                var n_user = _mapper.Map<UserEditViewModel, Hrmt01>(user, o_user);

                // update user roles
                // only checked role to update
                editUserRoles(user.UserNo, user.Roles.Where(r => r.CHECKED == true));

                _unitOfWork.Db.SaveChanges();

                resultCode = "edit user success";
            }
            catch (Exception ex)
            {
                resultCode = ex.Message;
                return false;
            }

            return true;
        }


    }
}
