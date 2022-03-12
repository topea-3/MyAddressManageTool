using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAddressManageTool.TableManager;
using MyAddressManageTool.View.HostInformationView;
using MyAddressManageTool.TableManager.Manager;
using MyAddressManageTool.TableManager.Mapper;
using MyAddressManageTool.TableManager.Entity;
using MyAddressManageTool.Core.ExceptionManage;
using MyAddressManageTool.MyApi;

namespace MyAddressManageTool.Model
{
    internal class HostInformationRegister
    {
        private const string WHERE_SQL = "where HOST_NAME = ? and DELETE_FLAG = '0' ";
        private const string DUPPLICATE_ERROR = "EA0002";

        // トランザクション
        private readonly TransactionManager transactionManager;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="transaction">トランザクション</param>
        public HostInformationRegister(TransactionManager transaction)
        {
            transactionManager = transaction;
        }

        /// <summary>
        /// 登録前チェック
        /// </summary>
        /// <param name="hostInformation"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="MyApplicationException"></exception>
        public void ApplicationArgCheck(HostInformation hostInformation)
        {
            // ホスト名称重複チェック
            TableManagerControll<HostInfoTableEntity> table = new(new HostInfoTableMapper(), transactionManager.Transaction);
            IList<object> parameter = new List<object>();
            parameter.Add(hostInformation.HostName?? throw new ArgumentNullException(nameof(hostInformation.HostName)));
            if (0 < table.Count(WHERE_SQL, parameter))
            {
                IList<string> messageParameter = new List<string>();
                messageParameter.Add(hostInformation.HostName);
                throw new MyApplicationException(DUPPLICATE_ERROR, messageParameter.ToArray());
            }
        }

        /// <summary>
        /// 登録処理
        /// </summary>
        /// <param name="hostInformation"></param>
        public void Register(HostInformation hostInformation)
        {
            // テーブルEntityへコピー
            HostInfoTableEntity tableEntity = new();
            PropertiesCopyUtil.CopyProperties(hostInformation, ref tableEntity, PropertiesCopyUtil.CopyType.NullEmptyOverride);

            // 採番
            tableEntity.HostId = NumberControllManager.GetNextNumber(transactionManager.Transaction, "HOST_ID");
            tableEntity.SeqNo = 1;
            tableEntity.DeleteFlag = "0";

            // 作成日時設定
            DateTime dateTime = DateTime.Now;
            tableEntity.UpdateDateTime = dateTime;
            tableEntity.CreateDateTime = dateTime;

            // 登録処理
            TableManagerControll<HostInfoTableEntity> table = new(new HostInfoTableMapper(), transactionManager.Transaction);
            table.Insert(tableEntity);
        }
    }
}
