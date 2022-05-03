using MyAddressManageTool.Core.ExceptionManage;
using MyAddressManageTool.Core.Message;
using MyAddressManageTool.Core.Message.Implement;
using MyAddressManageTool.MyApi;
using MyAddressManageTool.TableManager;
using MyAddressManageTool.TableManager.Entity;
using MyAddressManageTool.TableManager.Manager;
using MyAddressManageTool.TableManager.Mapper;
using MyAddressManageTool.View.AddressInfoView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAddressManageTool.Model
{
    internal class AddressInfoInquiry
    {
        // エラーID
        private const string CONDITION_ERROR = "EA0004";
        private const string UNIQ_ERROR = "ES0004";
        // SQL条件句共通
        private const string WHERE_CMN = "where ";
        private const string WHERE_CMN_AND = " AND ";
        private const string WHERE_CMN_NON_HISTORY = " DELETE_FLAG = '0' ";
        private const string REPLACE_WORD = "__BELONG_HOST_ID__";
        // SQL条件句住所情報
        private const string WHERE1_ADDRESS_ID = " ADDRESS_ID = ? ";
        private const string WHERE1_FAMILY_NAME = " FAMILY_NAME = ? ";
        private const string WHERE1_NAME = " NAME = ?";
        private const string WHERE1_SUB_NAME = " (SUB_NAME_1 = ? OR SUB_NAME_2 = ? OR SUB_NAME_3 = ? OR SUB_NAME_4 = ? OR SUB_NAME_5 = ?) ";
        private const string WHERE1_ADDRESS = " (ADDRESS_1 & ADDRESS_2) LIKE ? ";
        private const string WHERE1_HOST_ID = $" BELONG_HOST_ID IN ({REPLACE_WORD}) ";
        private const string WHERE1_NY_CREATE_FLAG = " NY_LETTER_CREATE_FLAG = ? ";
        private const string ORDER_BY = " ORDER BY ADDRESS_ID ASC, SEQ_NO ASC";
        // SQL条件句ホスト情報
        private const string WHERE2_HOST_INFO = $"WHERE HOST_NAME LIKE ? AND {WHERE_CMN_NON_HISTORY}";
        // SQL条件句ホスト情報2
        private const string WHERE3_HOST_INFO_UNIT = $"WHERE HOST_ID = ? AND {WHERE_CMN_NON_HISTORY}";

        // トランザクション
        private readonly TransactionManager transactionManager;
        private readonly TableManagerControll<HostInfoTableEntity> hostInfoTableManager;

        public AddressInfoInquiry(TransactionManager transaction)
        {
            transactionManager = transaction;
            hostInfoTableManager = new(new HostInfoTableMapper(), transactionManager.Transaction);
        }

        public void ApplicationArgCheck(AddressInfoSearchCondition condition)
        {
            if (!string.IsNullOrEmpty(condition.BelongHostId) && !string.IsNullOrEmpty(condition.BelongHostName))
            {
                IList<string> messageParameter = new List<string>();
                messageParameter.Add("担当ホストID");
                messageParameter.Add("担当ホスト名称");
                throw new MyApplicationException(CONDITION_ERROR, messageParameter.ToArray());
            }
        }

        public IList<Inquiry4AddressInformation> Search(AddressInfoSearchCondition condition)
        {
            // SQL構成
            IList<object> parameter = new List<object>();
            StringBuilder sql = new();
            bool isFirstCondition = true;
            // 住所ID
            if (!string.IsNullOrEmpty(condition.AddressId))
            {
                SqlBuilder(ref sql, WHERE1_ADDRESS_ID, ref isFirstCondition);
                parameter.Add(condition.AddressId);
            }
            // 姓
            if (!string.IsNullOrEmpty(condition.FamilyName))
            {
                SqlBuilder(ref sql, WHERE1_FAMILY_NAME, ref isFirstCondition);
                parameter.Add(condition.FamilyName);
            }
            // 名
            if (!string.IsNullOrEmpty(condition.Name))
            {
                SqlBuilder(ref sql, WHERE1_NAME, ref isFirstCondition);
                parameter.Add(condition.Name);
            }
            // 連名
            if (!string.IsNullOrEmpty(condition.SubName))
            {
                SqlBuilder(ref sql, WHERE1_SUB_NAME, ref isFirstCondition);
                for (int i = 0; i < 5; ++i) {
                    parameter.Add(condition.SubName);
                }
            }
            // 住所
            if (!string.IsNullOrEmpty(condition.Address))
            {
                SqlBuilder(ref sql, WHERE1_ADDRESS, ref isFirstCondition);
                parameter.Add($"%{condition.Address}%");
            }
            // ホストID
            if (!string.IsNullOrEmpty(condition.BelongHostId))
            {
                SqlBuilder(ref sql, CreateHostIdCondition(1), ref isFirstCondition);
                parameter.Add(condition.BelongHostId);
            }
            // ホスト名称
            if (!string.IsNullOrEmpty(condition.BelongHostName))
            {
                ICollection<HostInfoTableEntity> hostInfos = GetHostInfos(condition.BelongHostName);
                SqlBuilder(ref sql, CreateHostIdCondition(hostInfos.Count), ref isFirstCondition);
                foreach (HostInfoTableEntity hostInfo in hostInfos)
                {
                    parameter.Add(hostInfo.HostId ?? throw new ArgumentNullException(nameof(hostInfo.HostId)));
                }
            }
            // 年賀状作成対象
            if (!string.IsNullOrWhiteSpace(condition.NyLetterCreateFlag))
            {
                SqlBuilder(ref sql, WHERE1_NY_CREATE_FLAG, ref isFirstCondition);
                parameter.Add(condition.NyLetterCreateFlag);
            }
            // 履歴表示フラグ（削除フラグ）
            if (!condition.IsHistoryView)
            {
                SqlBuilder(ref sql, WHERE_CMN_NON_HISTORY, ref isFirstCondition);
            }
            // オーダーバイ
            _ = sql.Append(ORDER_BY);

            // データ検索
            TableManagerControll<AddressInfoTableEntity> tableManager = new(new AddressInfoTableMapper(), transactionManager.Transaction);
            ICollection<AddressInfoTableEntity> addressInfoTableEntities = tableManager.Select(sql.ToString(), parameter);

            // データ詰め替え
            IList<Inquiry4AddressInformation> inquiry4AddressInformationList = new List<Inquiry4AddressInformation>();
            foreach (AddressInfoTableEntity addressInfo in addressInfoTableEntities)
            {
                Inquiry4AddressInformation inquiry4Address = new();
                PropertiesCopyUtil.CopyProperties(addressInfo, ref inquiry4Address, PropertiesCopyUtil.CopyType.NullEmptyOverride);
                // 氏名
                inquiry4Address.Name = addressInfo.FamilyName + " " + addressInfo.Name;
                // 住所
                inquiry4Address.Address = addressInfo.Address1 + addressInfo.Address2;
                // 担当ホスト名称
                inquiry4Address.BelongHostName = GetHostName(addressInfo.BelongHostId ?? throw new ArgumentNullException(nameof(addressInfo.BelongHostId)));
                inquiry4AddressInformationList.Add(inquiry4Address);
            }

            return inquiry4AddressInformationList;
        }

        /// <summary>
        /// SQL構成
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <param name="sqlParts">SQL組み込み部品</param>
        /// <param name="isFirstCondition">第一部品フラグ</param>
        private void SqlBuilder(ref StringBuilder sql, string sqlParts, ref bool isFirstCondition)
        {
            if (isFirstCondition)
            {
                _ = sql.Append(WHERE_CMN);
                isFirstCondition = false;
            }
            else
            {
                _ = sql.Append(WHERE_CMN_AND);
            }
            _ = sql.Append(sqlParts);
        }

        /// <summary>
        /// ホストID条件句生成
        /// </summary>
        /// <param name="HostIdCount"></param>
        /// <returns></returns>
        private string CreateHostIdCondition(int HostIdCount)
        {
            bool isFirst = true;
            
            StringBuilder builder = new StringBuilder();

            if (HostIdCount == 0)
            {
                return builder.ToString();
            }

            for(int i = 0; i < HostIdCount; i++)
            {
                if (isFirst)
                {
                    builder.Append("?");
                    isFirst = false;
                }
                else
                {
                    builder.Append(", ?");
                }
            }

            return WHERE1_HOST_ID.Replace(REPLACE_WORD, builder.ToString());
        }

        /// <summary>
        /// ホスト情報検索
        /// </summary>
        /// <param name="hostName"></param>
        /// <returns></returns>
        private ICollection<HostInfoTableEntity> GetHostInfos(string hostName)
        {
            // 検索条件生成
            IList<object> parameter = new List<object>();
            parameter.Add($"%{hostName}%");
            // データ検索
            return hostInfoTableManager.Select(WHERE2_HOST_INFO, parameter);
        }

        /// <summary>
        /// ホスト名称取得
        /// </summary>
        /// <param name="hostId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        /// <exception cref="SystemException"></exception>
        private string? GetHostName(string hostId)
        {
            // 検索条件生成
            IList<object> parameter = new List<object>();
            parameter.Add(hostId);
            // データ検索
            ICollection<HostInfoTableEntity> hostInfos = hostInfoTableManager.Select(WHERE3_HOST_INFO_UNIT, parameter);

            if (hostInfos.Count == 0)
            {
                throw new NotFoundException(new HostInfoTableMapper().GetTableName());
            }

            if (hostInfos.Count > 1)
            {
                IList<string> psholders = new List<string>();
                psholders.Add(new HostInfoTableMapper().GetTableName());
                IMessageManager manager = new ErrorMessageManagerImpl();
                throw new SystemException(manager.GetMessage(UNIQ_ERROR, psholders.ToArray()));
            }

            return hostInfos.First().HostName;
        }
    }
}
