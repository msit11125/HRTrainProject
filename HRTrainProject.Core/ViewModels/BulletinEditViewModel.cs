using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRTrainProject.Core.ViewModels
{
    public class BulletinEditViewModel
    {
        public string CLASS_TYPE { get; set; }
        public string LANGUAGE_ID { get; set; }
        [Display(Name = "Bulletin_Class_Name")]
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

        public string MEMO { get; set; }
        public string CONTENT_TXT { get; set; }

    }
}
