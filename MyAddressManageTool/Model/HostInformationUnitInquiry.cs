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
    internal class HostInformationUnitInquiry
    {
        // トランザクション
        private readonly TransactionManager transactionManager;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="transaction">トランザクション</param>
        public HostInformationUnitInquiry(TransactionManager transaction)
        {
            transactionManager = transaction;
        }

        /// <summary>
        /// ホスト情報取得
        /// </summary>
        /// <param name="HostId"></param>
        /// <param name="seqNo"></param>
        /// <returns>ホスト情報</returns>
        public HostInformation GetHostInformation(string? hostId, int seqNo)
        {
            // 引数チェック
            if (null == hostId)
            {
                throw new ArgumentNullException(nameof(hostId));
            }

            // 検索条件取得
            HostInfoTableEntity argEntity = new();
            argEntity.HostId = hostId;
            argEntity.SeqNo = seqNo;

            // データ取得
            TableManagerControll<HostInfoTableEntity> table = new(new HostInfoTableMapper(), transactionManager.Transaction);
            HostInfoTableEntity returnEntity = table.FindByPrimarykey(argEntity);

            // 返却
            HostInformation hostInformation = new();
            PropertiesCopyUtil.CopyProperties(returnEntity, ref hostInformation, PropertiesCopyUtil.CopyType.NullEmptyOverride);
            // DateTimeコピー
            hostInformation.CreateDateTime = returnEntity.CreateDateTime;
            hostInformation.UpdateDateTime = returnEntity.UpdateDateTime;
            return hostInformation;
        }
    }
}
