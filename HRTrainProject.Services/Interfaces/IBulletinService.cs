using System.Collections.Generic;
using HRTrainProject.Core.ViewModels;
using HRTrainProject.Core.ViewModels.Filter;

namespace HRTrainProject.Services.Interfaces
{
    public interface IBulletinService
    {
        List<BulletinRow> GetAllBulletin(GetAllBulletinFilter filter);
    }
}