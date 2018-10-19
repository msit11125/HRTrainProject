using HRTrainProject.Core.Models;
using HRTrainProject.DAL;
using HRTrainProject.Services.Interfaces;
using HRTrainProject.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRTrainProject.Test
{
    public static class MockDb
    {
        /// <summary>
        /// Fake DbContext
        /// </summary>
        /// <returns></returns>
        public static HRTrainDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<HRTrainDbContext>()
                .UseInMemoryDatabase(databaseName: "HRTrainDb")
                .Options;
            var context = new HRTrainDbContext(options);

            Seed(context);

            return context;
        }

        /// <summary>
        /// Fake 種子
        /// </summary>
        /// <param name="context"></param>
        public static void Seed(HRTrainDbContext context)
        {
            var hrmtList = new List<HRMT01>()
            {
                new HRMT01(){ USER_NO="0000", NAME="SuperMan", PASSWORD = "1234"},
                new HRMT01(){ USER_NO="0001", NAME="Man", PASSWORD = "1234"}
            };

            context.HRMT01.AddRange(hrmtList);
            context.SaveChanges();
        }

        #region Moq Object
        public static DbSet<T> GetQueryableMockDbSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();
            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            dbSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>((s) => sourceList.Add(s));

            return dbSet.Object;
        }
        #endregion
    }
}
