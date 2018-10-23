using Dapper;
using HRTrainProject.Core;
using HRTrainProject.Core.ViewModels;
using HRTrainProject.Core.ViewModels.Filter;
using HRTrainProject.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRTrainProject.Services
{
    public class BulletinService : IBulletinService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BulletinService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public List<BulletinEditViewModel> GetAllBulletin(GetAllBulletinFilter filter)
        {
            var queryBulletin = _unitOfWork.Db.Database.GetDbConnection().Query<BulletinEditViewModel>
                (@"
                      select b2L.BULLET_ID, b2L.SUBJECT, b2.CLASS_TYPE, b1L.CLASS_NAME, 
                      b2.S_DATE, b2.E_DATE, b2.TOP_YN, b2.ISPUBLISH, b1L.LANGUAGE_ID, 
                      b2L.CRE_PERSON, b2L.CHG_DATE, b2L.CRE_PERSON, b2L.CHG_PERSON 
                      from BET02_LANG b2L
                      left join BET02 b2 on b2L.BULLET_ID = b2.BULLET_ID
                      left join BET01_LANG b1L on b2.CLASS_TYPE = b1L.CLASS_TYPE
                      where b1L.CLASS_TYPE = @CLASS_TYPE and b2L.LANGUAGE_ID = @LANGUAGE_ID
                      order by TOP_YN, b2L.CRE_DATE desc
                  "
                , new
                {
                    CLASS_TYPE = (int)filter.CLASS_TYPE,
                    LANGUAGE_ID = filter.LANGUAGE_ID
                });

            // 篩選 ...
            if (!string.IsNullOrEmpty(filter.QueryString))
            {
                queryBulletin = queryBulletin.Where(b => b.SUBJECT.Contains(filter.QueryString));
            }

            // On DashBoard ...
            if (filter.IsOnDashBoard)
            {
                // 單頁顯示筆數
                int showCount = _unitOfWork.Db.BET01
                    .Where(b => b.CLASS_TYPE == ((int)filter.CLASS_TYPE).ToString())
                    .FirstOrDefault()?.SHOW_COUNT ?? 0;

                queryBulletin = queryBulletin.Take(showCount);
                queryBulletin = queryBulletin.OrderByDescending(b => b.TOP_YN).ThenByDescending(b => b.CRE_DATE);

                return queryBulletin.ToList();
            }

            // 排序 ...
            if (filter.OrderColumn == null) // 預設排序
            {
                filter.OrderColumn = nameof(BulletinEditViewModel.CRE_DATE);
                filter.SortBy = SortBy.DESC;
            }
            var propertyInfo = typeof(BulletinEditViewModel).GetProperty(filter.OrderColumn);
            if (filter.SortBy == SortBy.ASC)
            {
                queryBulletin = queryBulletin.OrderBy(x => propertyInfo.GetValue(x, null));
            }
            else
            {
                queryBulletin = queryBulletin.OrderByDescending(x => propertyInfo.GetValue(x, null));
            }

            return queryBulletin.ToList();
        }

        public BulletinEditViewModel GetBulletinDetail(string bullet_id, string language_id, out string resultCode)
        {
            var bulletin = _unitOfWork.Db.Database.GetDbConnection().Query<BulletinEditViewModel>
               (@"
                      select b2L.BULLET_ID, b2L.SUBJECT, b2.CLASS_TYPE, b1L.CLASS_NAME, 
                      b2.S_DATE, b2.E_DATE, b2.TOP_YN, b2.ISPUBLISH, b1L.LANGUAGE_ID,
                      b2L.CRE_PERSON, b2L.CHG_DATE, b2L.CRE_PERSON, b2L.CHG_PERSON ,
                      b2.MEMO , b2L.CONTENT_TXT 
                      from BET02_LANG b2L
                      left join BET02 b2 on b2L.BULLET_ID = b2.BULLET_ID
                      left join BET01_LANG b1L on b2.CLASS_TYPE = b1L.CLASS_TYPE
                      where b2L.BULLET_ID = @BULLET_ID and b2L.LANGUAGE_ID = @LANGUAGE_ID
                      order by TOP_YN, b2L.CRE_DATE desc
                  "
               , new
               {
                   BULLET_ID = bullet_id,
                   LANGUAGE_ID = language_id
               }).FirstOrDefault();

            if(bulletin == null)
            {
                resultCode = "bullet_id not existed";
                return null;
            }

            resultCode = "get detail success";

            return bulletin;
        }

        public List<BulletinClassTypeModel> GetAllClassType(string language_id)
        {
            var classTypeList = _unitOfWork.Db.BET01_LANG.Where(bl => bl.LANGUAGE_ID == language_id).Select(s => new BulletinClassTypeModel()
            {
                CLASS_TYPE = s.CLASS_TYPE,
                CLASS_NAME = s.CLASS_NAME
            });

            return classTypeList.ToList();
        }

    }
}
