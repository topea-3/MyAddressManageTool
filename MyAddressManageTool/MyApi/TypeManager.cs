using System;
using System.Collections.Generic;
using MyAddressManageTool.TableManager;
using MyAddressManageTool.TableManager.Entity;
using MyAddressManageTool.TableManager.Manager;
using MyAddressManageTool.TableManager.Mapper;

namespace MyAddressManageTool.MyApi
{
    internal class TypeManager
    {
        private static readonly IDictionary<string, IDictionary<string, string?>?> typeDictionaryCash;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        static TypeManager()
        {
            // key TypeId, value value-valueName_Dictionary
            typeDictionaryCash = new Dictionary<string, IDictionary<string, string?>?>();
            if (typeDictionaryCash.Count == 0)
            {
                // データ取得
                TransactionManager transaction = new();
                transaction.StartTransaction();

                TableManagerControll<TypeDictTableEntity> tableManager 
                    = new(new TypeDictTableMapper(), transaction.Transaction);

                ICollection<TypeDictTableEntity> entityCollection
                    = tableManager.Select("order by TYPE_ID ASC, SORT ASC", new List<object>());

                // 前データ
                TypeDictTableEntity? befTypeDictTableEntity = null;
                // key value, value valueName
                IDictionary<string, string?>? typeValueNameDict = new Dictionary<string, string?>();
                // データキャッシュ
                foreach (TypeDictTableEntity entity in entityCollection)
                {
                    if(befTypeDictTableEntity == null)
                    {
                        //　初回処理
                        befTypeDictTableEntity = entity;
                        continue;
                    }

                    // value-valueName_Dictionaryに詰める
                    string value = befTypeDictTableEntity.Value ?? "";
                    typeValueNameDict.Add(value, befTypeDictTableEntity.ValueName);

                    if(befTypeDictTableEntity.TypeId != entity.TypeId)
                    {
                        // TypeId切り替わり時の処理
                        string typeId = befTypeDictTableEntity.TypeId ?? "";
                        typeDictionaryCash.Add(typeId, typeValueNameDict);
                        typeValueNameDict = new Dictionary<string, string?>();
                    }

                    // 前回情報に入れる
                    befTypeDictTableEntity= entity;
                }
                // 最後の処理
                string lastTypeId = befTypeDictTableEntity?.TypeId ?? "";
                typeDictionaryCash.Add(lastTypeId, typeValueNameDict);
            }
        }

        /// <summary>
        /// ValueNameセット取得
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public static IDictionary<string, string?>? GetTyepDictById(string typeId)
        {
            return typeDictionaryCash[typeId];
        }

        /// <summary>
        /// ValueNameセット取得(ブランクあり)
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public static IDictionary<string, string?>? GetTypeDictByIdWithBlank(string typeId)
        {
            IDictionary<string, string?>? returnDict = new Dictionary<string, string?>();
            returnDict.Add("", "");

            IDictionary<string, string?>? kvps = typeDictionaryCash[typeId];

            if (kvps == null)
            {
                return returnDict;
            }

            foreach (KeyValuePair<string, string?> kvp in kvps)
            {
                returnDict.Add(kvp.Key, kvp.Value);
            }

            return returnDict;
        }

        /// <summary>
        /// タイプ値名称取得
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="typeValue"></param>
        /// <returns></returns>
        public static string? GetValueName(string typeId, string typeValue)
        {
            IDictionary<string, string?>? typeValueNameDict = GetTyepDictById(typeId);
            return typeValueNameDict?[typeValue];
        }

        /// <summary>
        /// タイプ値名からタイプ値を取得
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string? GetValue(string typeId, string name)
        {
            IDictionary<string, string?>? typeValueNameDict = GetTyepDictById(typeId);

            if (typeValueNameDict == null)
            {
                return "";
            }

            foreach(var key in typeValueNameDict.Keys)
            {
                if (name == typeValueNameDict[key])
                {
                    return key;
                }
            }

            return "";
        }
    }
}
