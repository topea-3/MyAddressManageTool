using System.Collections.Generic;
using MyAddressManageTool.TableManager;
using MyAddressManageTool.TableManager.Entity;
using MyAddressManageTool.TableManager.Manager;
using MyAddressManageTool.TableManager.Mapper;

namespace MyAddressManageTool.MyApi
{
    internal class TypeManager
    {
        private static readonly IDictionary<string, IDictionary<string, string?>?> typeDictionaryCash
            = new Dictionary<string, IDictionary<string, string?>?>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public static void Init()
        {
            if (typeDictionaryCash.Count == 0)
            {
                // データ取得
                TransactionManager transaction = new();
                transaction.StartTransaction();

                TableManagerControll<TypeDictTableEntity> tableManager 
                    = new(new TypeDictTableMapper(), transaction.Transaction);

                ICollection<TypeDictTableEntity> entityCollection
                    = tableManager.Select("order by TYPE_ID ASC, SORT ASC", new List<object>());

                string bewforeTypeId = "";
                IDictionary<string, string?>? typeValueNameDict = new Dictionary<string, string?>();
                // データキャッシュ
                foreach (TypeDictTableEntity entity in entityCollection)
                {
                    if (!bewforeTypeId.Equals(entity.TypeId))
                    {
                        if (!string.IsNullOrEmpty(bewforeTypeId))
                        {
                            typeDictionaryCash.Add(bewforeTypeId, typeValueNameDict);
                        }
                        typeValueNameDict = new Dictionary<string, string?>();
                    }
                    typeValueNameDict.Add(entity.Value ?? "", entity.ValueName);
                }
                typeDictionaryCash.Add(bewforeTypeId, typeValueNameDict);
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
    }
}
