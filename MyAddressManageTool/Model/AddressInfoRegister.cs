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
using System.Windows;

namespace MyAddressManageTool.Model
{
    internal class AddressInfoRegister
    {
        private const string WHERE_SQL = "where FAMILY_NAME = ? and NAME = ? and DELETE_FLAG = '0' ";
        private const string DUPLICATE_WARNING_MESSAGE_ID = "M0009";

        // トランザクション
        private readonly TransactionManager transactionManager;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="transactionManager">トランザクション</param>
        public AddressInfoRegister(TransactionManager transactionManager)
        {
            this.transactionManager = transactionManager;
        }

        /// <summary>
        /// 登録前チェック
        /// </summary>
        /// <param name="addressInformation"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void ApplicationArgCheck(AddressInformation addressInformation)
        {
            // 姓名重複チェック（重複データ有の場合ワーニングポップアップ）
            TableManagerControll<AddressInfoTableEntity> table = new(new AddressInfoTableMapper(), transactionManager.Transaction);
            IList<object> parameter = new List<object>();
            parameter.Add(addressInformation.FamilyName ?? throw new ArgumentNullException(nameof(addressInformation.FamilyName)));
            parameter.Add(addressInformation.Name ?? throw new ArgumentNullException(nameof(addressInformation.Name)));
            if (0 < table.Count(WHERE_SQL, parameter))
            {
                IMessageManager messageManager = new MessageManagerImpl();
                string warningMessage = messageManager.GetMessage(DUPLICATE_WARNING_MESSAGE_ID
                    , new string[] { $"{addressInformation.FamilyName} {addressInformation.Name}" });
                string addInfo = "確認してください。";
                _ = MessageBox.Show(warningMessage + addInfo, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// 登録処理
        /// </summary>
        /// <param name="addressInformation"></param>
        public void Register(AddressInformation addressInformation)
        {
            // テーブルEntityへコピー
            AddressInfoTableEntity tableEntity = new();
            PropertiesCopyUtil.CopyProperties(addressInformation, ref tableEntity, PropertiesCopyUtil.CopyType.NullEmptyOverride);

            // 採番
            tableEntity.AddressId = NumberControllManager.GetNextNumber(transactionManager.Transaction, "ADDRESS_ID");
            tableEntity.SeqNo = 1;
            tableEntity.DeleteFlag = "0";

            // 作成日時設定
            DateTime dateTime = DateTime.Now;
            tableEntity.UpdateDateTime = dateTime;
            tableEntity.CreateDateTime = dateTime;

            // 登録処理
            TableManagerControll<AddressInfoTableEntity> table = new(new AddressInfoTableMapper(), transactionManager.Transaction);
            table.Insert(tableEntity);
        }
    }
}
