using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAddressManageTool.TableManager.Mapper
{
    internal class ReceiveInfoTableMapper : IMapper
    {
        // テーブル名
        private const string TABLE_NAME = "RECEIVE_INFO";

        /// <summary>
        /// マッパー定義
        /// </summary>
        private readonly IDictionary<string, IMapper.ItemProperties> fileldDict
            = new Dictionary<string, IMapper.ItemProperties>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ReceiveInfoTableMapper()
        {
            // マッパー内容定義
            fileldDict.Add("ReceiveNo", new IMapper.ItemProperties("RECEIVE_NO", true));
            fileldDict.Add("ReceiveDate", new IMapper.ItemProperties("RECEIVE_DATE"));
            fileldDict.Add("ReceiveCategoryType", new IMapper.ItemProperties("RECEIVE_CATEGORY_TYPE"));
            fileldDict.Add("ReceiveHostId", new IMapper.ItemProperties("RECEIVE_HOST_ID"));
            fileldDict.Add("PartyAddressId", new IMapper.ItemProperties("PARTY_ADDRESS_ID"));
            fileldDict.Add("Remarks", new IMapper.ItemProperties("REMARKS"));
            fileldDict.Add("DeleteFlag", new IMapper.ItemProperties("DELETE_FLAG"));
            fileldDict.Add("CreateDateTime", new IMapper.ItemProperties("CREATE_DATE_TIME"));
            fileldDict.Add("UpdateDateTime", new IMapper.ItemProperties("UPDATE_DATE_TIME"));
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
