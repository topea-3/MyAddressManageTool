using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAddressManageTool.TableManager.Mapper
{
    internal class TypeDictTableMapper : IMapper
    {
        // テーブル名
        private const string TABLE_NAME = "TYPE_DICT";

        /// <summary>
        /// マッパー定義
        /// </summary>
        private readonly IDictionary<string, IMapper.ItemProperties> fileldDict
            = new Dictionary<string, IMapper.ItemProperties>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TypeDictTableMapper()
        {
            // マッパー内容定義
            fileldDict.Add("TypeId", new IMapper.ItemProperties("TYPE_ID", true));
            fileldDict.Add("TypeName", new IMapper.ItemProperties("TYPE_NAME"));
            fileldDict.Add("Value", new IMapper.ItemProperties("VALUE", true));
            fileldDict.Add("ValueName", new IMapper.ItemProperties("VALUE_NAME"));
            fileldDict.Add("Sort", new IMapper.ItemProperties("SORT"));
        }

        public IDictionary<string, IMapper.ItemProperties> GetItemInfoDict()
        {
            return fileldDict;
        }

        public string GetTableName()
        {
            return TABLE_NAME;
        }
    }
}
