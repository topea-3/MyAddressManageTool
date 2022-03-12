using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAddressManageTool.TableManager.Mapper
{
    internal class AddressInfoTableMapper : IMapper
    {
        // テーブル名
        private const string TABLE_NAME = "ADDRESS_INFO";

        /// <summary>
        /// マッパー定義
        /// </summary>
        private readonly IDictionary<string, IMapper.ItemProperties> fileldDict
            = new Dictionary<string, IMapper.ItemProperties>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AddressInfoTableMapper()
        {
            // マッパー内容定義
            fileldDict.Add("AddressId", new IMapper.ItemProperties("ADDRESS_ID", true));
            fileldDict.Add("SeqNo", new IMapper.ItemProperties("SEQ_NO", true));
            fileldDict.Add("FamilyName", new IMapper.ItemProperties("FAMILY_NAME"));
            fileldDict.Add("Name", new IMapper.ItemProperties("NAME"));
            fileldDict.Add("HonorificTitle", new IMapper.ItemProperties("HONORIFIC_TITLE"));
            fileldDict.Add("SubName1", new IMapper.ItemProperties("SUB_NAME_1"));
            fileldDict.Add("SubName1Honor", new IMapper.ItemProperties("SUB_NAME_1_HONOR"));
            fileldDict.Add("SubName2", new IMapper.ItemProperties("SUB_NAME_2"));
            fileldDict.Add("SubName2Honor", new IMapper.ItemProperties("SUB_NAME_2_HONOR"));
            fileldDict.Add("SubName3", new IMapper.ItemProperties("SUB_NAME_3"));
            fileldDict.Add("SubName3Honor", new IMapper.ItemProperties("SUB_NAME_3_HONOR"));
            fileldDict.Add("SubName4", new IMapper.ItemProperties("SUB_NAME_4"));
            fileldDict.Add("SubName4Honor", new IMapper.ItemProperties("SUB_NAME_4_HONOR"));
            fileldDict.Add("SubName5", new IMapper.ItemProperties("SUB_NAME_5"));
            fileldDict.Add("SubName5Honor", new IMapper.ItemProperties("SUB_NAME_5_HONOR"));
            fileldDict.Add("AddressNumber1", new IMapper.ItemProperties("ADDRESS_NUMBER_1"));
            fileldDict.Add("AddressNumber2", new IMapper.ItemProperties("ADDRESS_NUMBER_2"));
            fileldDict.Add("Address1", new IMapper.ItemProperties("ADDRESS_1"));
            fileldDict.Add("Address2", new IMapper.ItemProperties("ADDRESS_2"));
            fileldDict.Add("BelongHostId", new IMapper.ItemProperties("BELONG_HOST_ID"));
            fileldDict.Add("NyLetterCreateFlag", new IMapper.ItemProperties("NY_LETTER_CREATE_FLAG"));
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
