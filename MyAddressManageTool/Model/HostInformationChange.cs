using MyAddressManageTool.Core.ExceptionManage;
using MyAddressManageTool.MyApi;
using MyAddressManageTool.TableManager;
using MyAddressManageTool.TableManager.Entity;
using MyAddressManageTool.TableManager.Manager;
using MyAddressManageTool.TableManager.Mapper;
using MyAddressManageTool.View.HostInformationView;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyAddressManageTool.Model
{
    internal class HostInformationChange
    {
        private const string WHERE_SQL = "where HOST_ID <> ? and HOST_NAME = ? and DELETE_FLAG = '0' ";
        private const string DUPPLICATE_ERROR = "EA0002";
        private const string NO_CHANGE_ERROR = "EA0003";
        private const string DELETE_FLAG_DELETE = "1";

        // トランザクション
        private readonly TransactionManager transactionManager;

        // テーブルコントロール
        TableManagerControll<HostInfoTableEntity> hostInfoTableManager;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="transaction">トランザクション</param>
        public HostInformationChange(TransactionManager transaction)
        {
            transactionManager = transaction;
            hostInfoTableManager = new(new HostInfoTableMapper(), transactionManager.Transaction);
        }

        public HostInformation ExcecuteChange(HostInformation argHostInformation)
        {
            // ホスト名称重複チェック
            DuplicateCheck(argHostInformation);

            // 変更前データ取得
            HostInfoTableEntity argHostInfoTableEntity4Update = new ();
            PropertiesCopyUtil.CopyProperties(argHostInformation, ref argHostInfoTableEntity4Update, PropertiesCopyUtil.CopyType.NullEmptyOverride);
            // DateTimeコピー
            argHostInfoTableEntity4Update.CreateDateTime = argHostInformation.CreateDateTime ?? DateTime.MinValue;
            argHostInfoTableEntity4Update.UpdateDateTime = argHostInformation.UpdateDateTime ?? DateTime.MinValue;
            HostInfoTableEntity beforeHostInfoTableEntity = hostInfoTableManager.FindByPrimarykey(argHostInfoTableEntity4Update);

            // 変更有無チェック
            if (!PropertiesCopyUtil.IsChangedProperties(argHostInfoTableEntity4Update, beforeHostInfoTableEntity))
            {
                throw new MyApplicationException(NO_CHANGE_ERROR, Array.Empty<string>());
            }

            // 最新データ履歴化
            beforeHostInfoTableEntity.DeleteFlag = DELETE_FLAG_DELETE;
            hostInfoTableManager.Update(beforeHostInfoTableEntity);

            // 更新データ追加
            argHostInfoTableEntity4Update.SeqNo++;
            argHostInfoTableEntity4Update.UpdateDateTime = DateTime.Now;
            hostInfoTableManager.Insert(argHostInfoTableEntity4Update);

            // 最新情報引きもどし
            HostInformation returnHostInformation = new();
            PropertiesCopyUtil.CopyProperties(argHostInfoTableEntity4Update, ref returnHostInformation, PropertiesCopyUtil.CopyType.NullEmptyOverride);
            // DateTimeコピー
            returnHostInformation.CreateDateTime = argHostInfoTableEntity4Update.CreateDateTime;
            returnHostInformation.UpdateDateTime = argHostInfoTableEntity4Update.UpdateDateTime;
            return returnHostInformation;
        }

        /// <summary>
        /// 登録前チェック
        /// </summary>
        /// <param name="hostInformation"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="MyApplicationException"></exception>
        private void DuplicateCheck(HostInformation hostInformation)
        {
            // ホスト名称重複チェック
            IList<object> parameter = new List<object>();
            parameter.Add(hostInformation.HostId ?? throw new ArgumentNullException(nameof(hostInformation.HostId)));
            parameter.Add(hostInformation.HostName ?? throw new ArgumentNullException(nameof(hostInformation.HostName)));

            if (0 < hostInfoTableManager.Count(WHERE_SQL, parameter))
            {
                IList<string> messageParameter = new List<string>();
                messageParameter.Add(hostInformation.HostName);
                throw new MyApplicationException(DUPPLICATE_ERROR, messageParameter.ToArray());
            }
        }

    }
}
