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
    internal class HostInformationDelete
    {
        private const string DELETE_FLAG_DELETE = "1";

        // トランザクション
        private readonly TransactionManager transactionManager;
        // テーブルコントロール
        private readonly TableManagerControll<HostInfoTableEntity> hostInfoTableManager;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="transaction">トランザクション</param>
        public HostInformationDelete(TransactionManager transaction)
        {
            this.transactionManager = transaction;
            hostInfoTableManager = new(new HostInfoTableMapper(), transactionManager.Transaction);
        }

        /// <summary>
        /// 削除処理
        /// </summary>
        /// <param name="argHostInformation"></param>
        /// <returns></returns>
        public HostInformation ExecuteDelete(HostInformation argHostInformation)
        {
            // データ削除
            HostInfoTableEntity argHostInfoTableEntity4Update = new();
            PropertiesCopyUtil.CopyProperties(argHostInformation, ref argHostInfoTableEntity4Update, PropertiesCopyUtil.CopyType.NullEmptyOverride);
            // DateTimeコピー
            argHostInfoTableEntity4Update.CreateDateTime = argHostInformation.CreateDateTime ?? DateTime.MinValue;
            argHostInfoTableEntity4Update.UpdateDateTime = argHostInformation.UpdateDateTime ?? DateTime.MinValue;
            
            // 削除更新
            argHostInfoTableEntity4Update.DeleteFlag = DELETE_FLAG_DELETE;
            argHostInfoTableEntity4Update.UpdateDateTime = DateTime.Now;
            hostInfoTableManager.Update(argHostInfoTableEntity4Update);

            // 最新情報引きもどし
            HostInformation returnHostInformation = new();
            PropertiesCopyUtil.CopyProperties(argHostInfoTableEntity4Update, ref returnHostInformation, PropertiesCopyUtil.CopyType.NullEmptyOverride);
            // DateTimeコピー
            returnHostInformation.CreateDateTime = argHostInfoTableEntity4Update.CreateDateTime;
            returnHostInformation.UpdateDateTime = argHostInfoTableEntity4Update.UpdateDateTime;
            return returnHostInformation;
        }
    }
}
