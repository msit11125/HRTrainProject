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

        public List<BulletinRow> GetAllBulletin(GetAllBulletinFilter filter)
        {
            var queryBulletin = _unitOfWork.Db.Database.GetDbConnection().Query<BulletinRow>
                (@"
                      select b2L.BULLET_ID, b2L.SUBJECT, b2.CLASS_TYPE, b1L.CLASS_NAME, 
                      b2.S_DATE, b2.E_DATE, b2.TOP_YN, b2.ISPUBLISH, b1L.LANGUAGE_ID
                      , b2L.CRE_PERSON, b2L.CHG_DATE, b2L.CRE_PERSON, b2L.CHG_PERSON,
                      b2.PUSH_YN from BET02_LANG b2L
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
            if (filter.CLASS_TYPE != 0)
            {
                queryBulletin = queryBulletin
                    .Where(u => u.CLASS_TYPE == ((int)filter.CLASS_TYPE).ToString());
            }
            if (filter.IsMore)
            {
                // 單頁顯示筆數
                int showCount = _unitOfWork.Db.BET01
                    .Where(b => b.CLASS_TYPE == ((int)filter.CLASS_TYPE).ToString())
                    .FirstOrDefault()?.SHOW_COUNT ?? 0;

                queryBulletin.Take(showCount);
            }

            // 排序 ...
            if (filter.OrderColumn == null) // 預設排序
            {
                filter.OrderColumn = nameof(BulletinRow.CRE_DATE);
                filter.SortBy = SortBy.DESC;
            }
            var propertyInfo = typeof(BulletinRow).GetProperty(filter.OrderColumn);
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

    }
}
