using System;
using System.Collections.Generic;
using System.Text;

namespace HRTrainProject.Services.Extensions
{
    public static class StringExtension
    {
        /// <summary>
        /// 字串轉Enum型別
        /// </summary>
        /// <typeparam name="T">Enum型別</typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this string value) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
