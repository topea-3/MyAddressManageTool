using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAddressManageTool.TableManager.Mapper
{
    internal class SendInfoTableMapper : IMapper
    {
        // テーブル名
        private const string TABLE_NAME = "SEND_INFO";

        /// <summary>
        /// マッパー定義
        /// </summary>
        private readonly IDictionary<string, IMapper.ItemProperties> fileldDict
            = new Dictionary<string, IMapper.ItemProperties>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SendInfoTableMapper()
        {
            // マッパー内容定義
            fileldDict.Add("SendNo", new IMapper.ItemProperties("SEND_NO", true));
            fileldDict.Add("SendDate", new IMapper.ItemProperties("SEND_DATE"));
            fileldDict.Add("SendCategoryType", new IMapper.ItemProperties("SEND_CATEGORY_TYPE"));
            fileldDict.Add("SendHostId", new IMapper.ItemProperties("SEND_HOST_ID"));
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
