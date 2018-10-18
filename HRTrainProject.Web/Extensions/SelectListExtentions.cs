using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace HRTrainProject.Web
{
    public static class SelectListExtentions
    {
        /// <summary>
        /// 集合轉換下拉SelectList - 1
        /// </summary>
        /// <param name="dataValueField">Value值欄位</param>
        /// <param name="dataTextField">Text值欄位</param>
        /// <returns></returns>
        public static SelectList ToSelectList<TSource, TValue, TText>(
            this IEnumerable<TSource> source,
            Expression<Func<TSource, TValue>> dataValueField,
            Expression<Func<TSource, TText>> dataTextField)
        {
            string dataName = ExpressionHelper.GetExpressionText(dataValueField);
            string textName = ExpressionHelper.GetExpressionText(dataTextField);

            return new SelectList(source, dataName, textName);
        }

        /// <summary>
        /// 集合轉換下拉SelectList - 2
        /// </summary>
        /// <param name="dataValueField">Value值欄位</param>
        /// <param name="dataTextField">Text值欄位</param>
        /// <param name="defaultValue">預設Selected值</param>
        /// <returns></returns>
        public static SelectList ToSelectList<TSource, TValue, TText>(
            this IEnumerable<TSource> source,
            Expression<Func<TSource, TValue>> dataValueField,
            Expression<Func<TSource, TText>> dataTextField, TValue defaultValue)
        {
            string dataName = ExpressionHelper.GetExpressionText(dataValueField);
            string textName = ExpressionHelper.GetExpressionText(dataTextField);

            return new SelectList(source, dataName, textName, defaultValue);
        }

    }
}
