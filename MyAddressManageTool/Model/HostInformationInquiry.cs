using MyAddressManageTool.MyApi;
using MyAddressManageTool.TableManager;
using MyAddressManageTool.TableManager.Entity;
using MyAddressManageTool.TableManager.Manager;
using MyAddressManageTool.TableManager.Mapper;
using MyAddressManageTool.View.HostInformationView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAddressManageTool.Model
{
    internal class HostInformationInquiry
    {
        // SQL条件句
        private const string WHERE = "where ";
        private const string WHERE_AND = " AND ";
        private const string WHERE_HOST_NAME = " HOST_NAME like ? ";
        private const string WHERE_NON_HISTORY = " DELETE_FLAG = '0' ";
        private const string WHERE_ORDER_BY = " ORDER BY HOST_ID ASC, SEQ_NO ASC";

        // トランザクション
        private readonly TransactionManager transactionManager;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="transactions">トランザクション</param>
        public HostInformationInquiry(TransactionManager transactions)
        {
            transactionManager = transactions;
        }

        /// <summary>
        /// 引数事前チェック
        /// </summary>
        /// <param name="condition"></param>
        public void ApplicationArgCheck(HostInfoSearchCondition condition)
        {
            // NOP
        }

        public IList<Inquiry4HostInformation> Search(HostInfoSearchCondition condition)
        {
            // ホスト情報取得
            TableManagerControll<HostInfoTableEntity> table = new(new HostInfoTableMapper(), transactionManager.Transaction);
            
            // SQL構成
            IList<object> parameter = new List<object>();
            StringBuilder sql = new();
            bool isFirstCondition = true;
            // ホスト名称
            if (!String.IsNullOrEmpty(condition.HostName))
            {
                SqlBuilder(ref sql, WHERE_HOST_NAME, isFirstCondition);
                parameter.Add($"%{condition.HostName}%");
                isFirstCondition = false;
            }
            // 履歴表示フラグ（削除フラグ）
            if (!condition.IsHistoryView)
            {
                SqlBuilder(ref sql, WHERE_NON_HISTORY, isFirstCondition);
            }
            // オーダーバイ
            _ = sql.Append(WHERE_ORDER_BY);

            // データ検索
            ICollection<HostInfoTableEntity> hostInfoTableEntities = table.Select(sql.ToString(), parameter);

            // データ詰め替え
            IList<Inquiry4HostInformation> inquiry4HostInformationList = new List<Inquiry4HostInformation>();
            foreach (HostInfoTableEntity hostInfoTableEntity in hostInfoTableEntities)
            {
                Inquiry4HostInformation inquiry4Host = new();
                PropertiesCopyUtil.CopyProperties(
                    hostInfoTableEntity, ref inquiry4Host, PropertiesCopyUtil.CopyType.NullEmptyOverride);
                inquiry4HostInformationList.Add(inquiry4Host);
            }

            return inquiry4HostInformationList;
        }

        /// <summary>
        /// SQL構成
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <param name="sqlParts">SQL組み込み部品</param>
        /// <param name="isFirstCondition">第一部品フラグ</param>
        private void SqlBuilder(ref StringBuilder sql, string sqlParts, bool isFirstCondition)
        {
            if (isFirstCondition)
            {
                _ = sql.Append(WHERE);
            } else {
                _ = sql.Append(WHERE_AND);
            }
            _ = sql.Append(sqlParts);
        }
    }
}
