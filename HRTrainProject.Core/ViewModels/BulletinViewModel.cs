using System;
using System.Collections.Generic;
using System.Text;
using X.PagedList;

namespace HRTrainProject.Core.ViewModels
{
    public class BulletinViewModel
    {
        public IPagedList<BulletinRow> BulletinList { get; set; }
    }

    public class BulletinRow
    {
        public string CLASS_TYPE { get; set; }
        public string LANGUAGE_ID { get; set; }
        public string CLASS_NAME { get; set; }

        public string BULLET_ID { get; set; }
        public string SUBJECT { get; set; }

        public DateTime? S_DATE { get; set; }
        public DateTime? E_DATE { get; set; }

        public string TOP_YN { get; set; }
        public string ISPUBLISH { get; set; }
        public string PUSH_YN { get; set; }
        public DateTime? CRE_DATE { get; set; }
        public DateTime? CHG_DATE { get; set; }

        public string CRE_PERSON { get; set; }
        public string CHG_PERSON { get; set; }

    }
}
