using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HRTrainProject.Services.Logics
{
    public class LogicCenter
    {
        /// <summary>
        /// 取得列舉Discription內容
        /// </summary>
        /// <param name="value">enum</param>
        /// <returns></returns>
        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        /// <summary>
        /// 取得列舉名稱
        /// </summary>
        /// <typeparam name="T">型別</typeparam>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string GetEnumName<T>(T e)
        {
            if (!typeof(T).IsEnum) throw new TypeAccessException("不是Enum型別");
            return Enum.GetName(typeof(T), e);
        }

        /// <summary>
        /// 列舉轉SelectList
        /// </summary>
        /// <typeparam name="T">型別</typeparam>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> GetEnumSelectList<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().Select(
                enu => new SelectListItem() { Text = Enum.GetName(typeof(T), enu), Value = enu.ToString() });
        }

        /// <summary>
        /// 集合(陣列)比較算法，比較兩集合是否相同。 
        /// [前置作業: 需先行排序]。
        /// </summary>
        /// <typeparam name="L">左元素型別</typeparam>
        /// <typeparam name="R">右元素型別</typeparam>
        /// <param name="lefts">左元素陣列</param>
        /// <param name="rights">右元素陣列</param>
        /// <param name="compare">比較公式</param>
        /// <returns></returns>
        public static bool IsTheSame<L, R>(IEnumerable<L> lefts, IEnumerable<R> rights, Func<L, R, bool> compare)
        {
            if (lefts.Count() != rights.Count())
                return false;

            int i = 0, j = 0;
            foreach (var l_item in lefts)
            {
                foreach (var r_item in rights)
                {
                    if (i == j)
                    {
                        if (!compare(l_item, r_item))
                            return false;

                        i = 0;
                        j++;
                        break;
                    }
                    i++;
                }
            }
            return true;
        }

        /// <summary>
        /// 讀取xml config檔(兩層) => 轉Dictionary
        /// </summary>
        /// <param name="path">完整路徑</param>
        /// <param name="descendantsParent">父節點</param>
        /// <param name="descendantsChild">子節點</param>
        /// <returns></returns>
        public static Dictionary<string, string> LoadXML(string path, string descendantsParent, string descendantsChild)
        {
            return XDocument.Load(path).Descendants(descendantsParent).
            Descendants(descendantsChild).ToDictionary(p => p.Attribute("key").Value,
            p => p.Attribute("value").Value);
        }

        /// <summary>
        /// 深度物件複製
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }
    }
}
