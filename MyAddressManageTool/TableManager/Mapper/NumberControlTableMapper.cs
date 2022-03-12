using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAddressManageTool.TableManager.Mapper
{
    internal class NumberControlTableMapper : IMapper
    {
        
        // テーブル名
        private const string TABLE_NAME = "NUBER_CONTROL";

        /// <summary>
        /// マッパー定義
        /// </summary>
        private readonly IDictionary<string, IMapper.ItemProperties> fileldDict 
            = new Dictionary<string, IMapper.ItemProperties>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NumberControlTableMapper()
        {
            // マッパー内容定義
            fileldDict.Add("Id", new IMapper.ItemProperties("ID",true));
            fileldDict.Add("NumberName", new IMapper.ItemProperties("NUMBER_NAME"));
            fileldDict.Add("CurrentNumber", new IMapper.ItemProperties("CURRENT_NUMBER"));
            fileldDict.Add("MaxNumber", new IMapper.ItemProperties("MAX_NUMBER"));
            fileldDict.Add("PreFix", new IMapper.ItemProperties("PREFIX"));
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
