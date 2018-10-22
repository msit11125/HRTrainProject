using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRTrainProject.Core.ViewModels.Filter
{
    public class BaseFilter
    {
        private int? actualPage;
        /// <summary>
        /// 選取頁面
        /// </summary>
        public int Page {
            get { return actualPage == null || actualPage <= 0 ? 1 : actualPage.Value; }
            set { actualPage = value; } }

        /// <summary>
        /// 單頁顯示筆數
        /// </summary>
        public int PageSize { get; set; } = 15;

        /// <summary>
        /// 查詢關鍵字
        /// </summary>
        public string QueryString { get; set; }

        /// <summary>
        /// 排序條件
        /// </summary>
        public string OrderColumn { get; set; }

        /// <summary>
        /// 排序方向
        /// </summary>
        public SortBy SortBy { get; set; }

        /// <summary>
        /// 按鈕權限
        /// </summary>
        public List<BtnPermission> BtnPermissions { get; set; }

        /// <summary>
        /// 檢查有無此權限
        /// </summary>
        /// <param name="btnPermission">權限按鈕代碼</param>
        /// <returns></returns>
        public bool HasPermission(BtnPermission btnPermission)
        {
            return this.BtnPermissions.Any(b => b == btnPermission);
        }

        public BaseFilter()
        {
            BtnPermissions = new List<BtnPermission>();
        }
    }
}
