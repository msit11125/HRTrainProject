using System;
using System.Collections.Generic;
using System.Text;

namespace HRTrainProject.Core.ViewModels.Filter
{
    public class GetAllBulletinFilter : BaseFilter
    {
        /// <summary>
        /// more page
        /// </summary>
        public bool IsMore { get; set; }
        public Bulletin_Class_Type CLASS_TYPE { get; set; }
        public string LANGUAGE_ID { get; set; }
    }
}
