using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAddressManageTool.MyApi
{
    /// <summary>
    /// プロパティユーティリティ
    /// </summary>
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

        // 許容型リスト
        private static readonly IList<Type> POSSIBLE_TYPE_LIST;

        /// <summary>
        /// staticコンストラクタ
        /// </summary>
        static PropertiesCopyUtil()
        {
            // 許容型リスト生成
            POSSIBLE_TYPE_LIST = new List<Type>();
            POSSIBLE_TYPE_LIST.Add(typeof(byte));
            POSSIBLE_TYPE_LIST.Add(typeof(sbyte));
            POSSIBLE_TYPE_LIST.Add(typeof(int));
            POSSIBLE_TYPE_LIST.Add(typeof(uint));
            POSSIBLE_TYPE_LIST.Add(typeof(short));
            POSSIBLE_TYPE_LIST.Add(typeof(ushort));
            POSSIBLE_TYPE_LIST.Add(typeof(long));
            POSSIBLE_TYPE_LIST.Add(typeof(ulong));
            POSSIBLE_TYPE_LIST.Add(typeof(float));
            POSSIBLE_TYPE_LIST.Add(typeof(double));
            POSSIBLE_TYPE_LIST.Add(typeof(char));
            POSSIBLE_TYPE_LIST.Add(typeof(string));
            POSSIBLE_TYPE_LIST.Add(typeof(bool));
            POSSIBLE_TYPE_LIST.Add(typeof(decimal));
            POSSIBLE_TYPE_LIST.Add(typeof(DateTime));
            POSSIBLE_TYPE_LIST.Add(typeof(DateOnly));
            POSSIBLE_TYPE_LIST.Add(typeof(DateTime?));
            POSSIBLE_TYPE_LIST.Add(typeof(DateOnly?));

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

                // コピー対象型チェック
                if (!POSSIBLE_TYPE_LIST.Contains(soucePropertyType))
                {
                    // 許容型以外の場合、対象外
                    continue;
                }

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

        /// <summary>
        /// プロパティ変更チェック
        /// </summary>
        /// <typeparam name="T">比較する型</typeparam>
        /// <param name="prop1">比較対象1</param>
        /// <param name="prop2">比較対象2</param>
        /// <returns>変更有の場合true</returns>
        public static bool IsChangedProperties<T>(T prop1, T prop2)
            where T : class 
        {
            Type prop1Type = prop1.GetType ();
            Type prop2Type = prop2.GetType ();

            foreach(System.Reflection.PropertyInfo? prop1Info in prop1Type.GetProperties())
            {
                // プロパティ名
                string propertyName = prop1Info.Name;

                // prop1情報
                Type prop1PropertyType = prop1Info.PropertyType;

                // 型チェック
                // コピー対象型チェック
                if (!POSSIBLE_TYPE_LIST.Contains(prop1PropertyType))
                {
                    // 許容型以外の場合スキップ
                    continue;
                }

                // prop2情報
                System.Reflection.PropertyInfo? prop2Info = prop2Type.GetProperty(propertyName);

                if (null == prop2Info)
                {
                    // 情報が取得できなかった場合スキップ
                    continue;
                }

                // 設定値比較
                object? prop1PropertyValue = prop1Info.GetValue(prop1);
                object? prop2PropertyValue = prop2Info.GetValue(prop2);
                if (!System.Object.Equals(prop1PropertyValue, prop2PropertyValue))
                {
                    // 変更があればtrueを返却
                    return true;
                }
            }

            // 変更がなければfalseを返却
            return false;
        }
    }
}
