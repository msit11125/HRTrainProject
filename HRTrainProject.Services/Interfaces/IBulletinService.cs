using System.Collections.Generic;
using HRTrainProject.Core.ViewModels;
using HRTrainProject.Core.ViewModels.Filter;

namespace HRTrainProject.Services.Interfaces
{
    public interface IBulletinService
    {
        List<BulletinEditViewModel> GetAllBulletin(GetAllBulletinFilter filter);
        BulletinEditViewModel GetBulletinDetail(string bullet_id, string language_id, out string resultCode);
    }
}