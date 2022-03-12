using System.Collections.Generic;

namespace MyAddressManageTool.TableManager
{
    internal interface IMapper
    {
        /// <summary>
        /// マッパー情報取得
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, ItemProperties> GetItemInfoDict();

        /// <summary>
        /// テーブル名取得
        /// </summary>
        /// <returns></returns>
        public  string GetTableName();

        /// <summary>
        /// テーブル項目情報
        /// </summary>
        public struct ItemProperties
        {
            /// <summary>
            /// コンストラクタ。
            /// </summary>
            /// <param name="itemName"></param>
            /// <param name="uniqKeyFlag"></param>
            public ItemProperties(string itemName, bool uniqKeyFlag = false)
            {
                ColumnName = itemName;
                UniqKeyFlag = uniqKeyFlag;
            }

            public string ColumnName { get; set; }
            public bool UniqKeyFlag { get; set; }
        }
    }

}
