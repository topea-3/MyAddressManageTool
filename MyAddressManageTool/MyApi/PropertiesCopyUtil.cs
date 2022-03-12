using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAddressManageTool.MyApi
{
    internal class PropertiesCopyUtil
    {
        /// <summary>
        /// コピータイプ
        /// </summary>
        public enum CopyType
        {
            NullEmptyOverride,
            NullEmptySkip
        }

        /// <summary>
        /// プロパティコピーメソッド（階層構造は対象外）
        /// </summary>
        /// <typeparam name="T">コピー元の型</typeparam>
        /// <typeparam name="K">コピー先の型</typeparam>
        /// <param name="souce">コピー元</param>
        /// <param name="target">コピー先</param>
        /// <param name="copyType">コピータイプ</param>
        public static void CopyProperties<T, K> (T souce, ref K target, CopyType copyType)
            where T : class where K : class
        {
            Type souceType = souce.GetType ();
            Type targetType = target.GetType ();

            foreach (System.Reflection.PropertyInfo? souceProperty in souceType.GetProperties ())
            {
                // プロパティ名
                string propertyName = souceProperty.Name;

                // コピー元情報
                Type soucePropertyType = souceProperty.PropertyType;
                object? soucePropertyValue = souceProperty.GetValue(souce);

                if (CopyType.NullEmptySkip.Equals(copyType))
                {
                    // NullEmptySkipの場合
                    if (typeof(string) == soucePropertyType)
                    {
                        string? soucePropertyValueString = Convert.ToString(soucePropertyValue);
                        
                        if (string.IsNullOrEmpty(soucePropertyValueString))
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (null == soucePropertyValue)
                        {
                            continue;
                        }
                    }
                }

                // コピー処理
                System.Reflection.PropertyInfo? targetProperty = targetType.GetProperty(propertyName);

                if (null == targetProperty)
                {
                    // プロパティが見つからなかった場合
                    continue;
                }

                if (soucePropertyType != targetProperty.PropertyType)
                {
                    // 型が一致しない場合
                    continue;
                }

                targetProperty.SetValue(target, soucePropertyValue);
            }
        }
    }
}
