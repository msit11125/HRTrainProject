using System;
using System.Collections.Generic;
using System.Text;

namespace HRTrainProject.Services.Extensions
{
    public static class ObjectExtension
    {
        /// <summary>
        /// 任意object轉任意型別
        /// </summary>
        /// <typeparam name="T">型別</typeparam>
        /// <param name="dataObj">物件Object</param>
        /// <returns></returns>
        public static T ToType<T>(this object dataObj)
        {
            try
            {
                var underlyingType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T); // 堤防Nullable發生Errror
                return (dataObj == DBNull.Value ? default(T) : (T)Convert.ChangeType(dataObj, underlyingType));
            }
            catch (Exception)
            {
                return default(T);
            }
        }
    }
}
