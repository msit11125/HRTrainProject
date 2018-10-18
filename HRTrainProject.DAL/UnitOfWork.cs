using HRTrainProject.Core.Models;
using HRTrainProject.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRTrainProject.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        public HRTrainDbContext Db { get; }

        public UnitOfWork(HRTrainDbContext context)
        {
            this.Db = context;
        }

        public void Complete()
        {
            Db.SaveChanges();
        }

        public async Task CompleteAsync()
        {
            await Db.SaveChangesAsync();
        }
    }
}
