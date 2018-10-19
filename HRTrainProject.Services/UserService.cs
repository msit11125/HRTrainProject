using AutoMapper;
using HRTrainProject.Core.Models;
using HRTrainProject.Services.Interfaces;
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
        private readonly IGenericRepository<HRMT01> _userRepo;
        private readonly IGenericRepository<HRMT24> _roleRepo;
       
        public UserService(IUnitOfWork unitOfWork, IMapper mapper,
            IGenericRepository<HRMT01> userDAL, IGenericRepository<HRMT24> roleDAL)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
            this._userRepo = userDAL;
            this._roleRepo = roleDAL;
        }

        public UserProfile LogIn(LogInModel user, out string resultCode)
        {
            UserProfile userProfile = null;
            var o_user = _userRepo.Get(u => u.USER_NO == user.USER_NO).FirstOrDefault();
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

            userProfile = _mapper.Map<HRMT01, UserProfile>(o_user);
            userProfile.Roles = this.GetUserRoles(user.USER_NO);

            resultCode = "loginSuccess";
            logger.Trace($@"{user.USER_NO} 登入系統成功。");

            return userProfile;
        }

        public List<UserRoles> GetUserRoles(string userNo)
        {
            var query_userRole = _unitOfWork.Db.HRMT25
                .Join(_unitOfWork.Db.HRMT24, uur => uur.ROLE_ID, r => r.ROLE_ID, 
                (uur, r) => new { uur.USER_NO, uur.ROLE_ID, r.ROLE_NAME, r.ROLE_TYPE, r.ROLE_LEVEL })
                .Where(w=>w.USER_NO == userNo)
                .AsEnumerable()
                .Select(s => new UserRoles
                {
                    ROLE_ID = s.ROLE_ID.ToEnum<UserRole>(),
                    ROLE_NAME = s.ROLE_NAME,
                    ROLE_TYPE = s.ROLE_TYPE,
                    ROLE_LEVEL = s.ROLE_LEVEL
                });

            return query_userRole.ToList();
        }

        public List<UserEditViewModel> GetUserList(GetUserListFilter filter)
        {
            var queryUser = _userRepo.Get();

            // 篩選 ...
            if (filter.QueryString != null)
            {
                queryUser = queryUser.Where(u => u.USER_NO.Contains(filter.QueryString));
            }
            if(filter.SearchName != null)
            {
                queryUser = queryUser.Where(u => u.NAME.Contains(filter.SearchName));
            }

            // 排序 ...
            if (filter.OrderColumn == null) // 預設排序
            {
                filter.OrderColumn = nameof(HRMT01.USER_NO);
                filter.SortBy = SortBy.ASC;
            }
            var propertyInfo = typeof(HRMT01).GetProperty(filter.OrderColumn);
            if (filter.SortBy == SortBy.ASC)
            {
                queryUser = queryUser.OrderBy(x => propertyInfo.GetValue(x, null));
            }
            else
            {
                queryUser = queryUser.OrderByDescending(x => propertyInfo.GetValue(x, null));
            }

            var queryUserList = queryUser.ToList();
            var userProfiles = _mapper.Map<List<HRMT01>, List<UserEditViewModel>>(queryUserList);

            return userProfiles;
        }

        public UserEditViewModel GetUserDetail(string userNo)
        {
            var ouser = _userRepo.Get(u => u.USER_NO == userNo).FirstOrDefault()?? new HRMT01();
            var userDetail = _mapper.Map<HRMT01, UserEditViewModel>(ouser);
           
            var query_userRole =  _unitOfWork.Db.Database.GetDbConnection().Query<RolesViewModel>
                (@"select r.ROLE_ID, r.ROLE_NAME, r.ROLE_LEVEL, r.ROLE_TYPE, ur.CHG_DATE, ur.CHG_PERSON, ur.DEFAULT_YN,
                           CAST(
                           case when  ur.USER_NO is null then 0 else 1 end 
                           AS BIT) as CHECKED
                             from HRMT24 r
                           left join (select * from HRMT25 where USER_NO =@USER_NO) ur 
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
                var n_user = _mapper.Map<UserEditViewModel, HRMT01>(user);
                n_user.CHG_DATE = DateTime.Now;
                n_user.CHG_PERSON = changer;

                _unitOfWork.Db.HRMT01.Add(n_user);
                _unitOfWork.Db.HRMT25.AddRange(
                    user.Roles.Where(r => r.CHECKED == true).Select(ur => new HRMT25()
                    {
                        ROLE_ID = ((int)ur.ROLE_ID).ToString(),
                        USER_NO = user.USER_NO,
                        CHG_DATE = DateTime.Now,
                        CHG_PERSON = changer,
                        DEFAULT_YN = ""
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
                var o_user_role = _unitOfWork.Db.HRMT25.Where(u => u.USER_NO == userNo).OrderBy(r => r.ROLE_ID).ToList();
                if (!LogicCenter.IsTheSame(
                    userRoles,
                    o_user_role,
                    (ur, our) => ((int)ur.ROLE_ID).ToString() == our.ROLE_ID))
                {
                    _unitOfWork.Db.HRMT25.RemoveRange(o_user_role);
                    _unitOfWork.Db.HRMT25.AddRange(userRoles.Select(ur => new HRMT25()
                    {
                        ROLE_ID = ((int)ur.ROLE_ID).ToString(),
                        USER_NO = userNo,
                        CHG_DATE = DateTime.Now,
                        CHG_PERSON = changer,
                        DEFAULT_YN = ""
                    }));
                }
            };

            try
            {
                // update user infos
                var o_user = _userRepo.Get(u => u.USER_NO == user.USER_NO).FirstOrDefault();
                if (o_user == null)
                {
                    resultCode = "notFindUser";
                    return false;
                }
                    
                if (string.IsNullOrEmpty(user.PHOTO)) // 圖片沒換就不更新，一樣使用舊圖
                    user.PHOTO = o_user.PHOTO;

                var n_user = _mapper.Map<UserEditViewModel, HRMT01>(user, o_user);
                n_user.CHG_DATE = DateTime.Now;
                n_user.CHG_PERSON = changer;
                // update user roles
                // only checked role to update
                editUserRoles(user.USER_NO, user.Roles.Where(r => r.CHECKED == true));

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
