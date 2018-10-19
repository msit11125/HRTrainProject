using HRTrainProject.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRTrainProject.Services.Interfaces
{
    public interface IUnitOfWork
    {
        HRTrainDbContext Db { get; }
        void Complete();
        Task CompleteAsync();
    }
}
