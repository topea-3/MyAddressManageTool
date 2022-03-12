using System;
using System.Data.OleDb;
using MyAddressManageTool.TableManager.Manager;
using MyAddressManageTool.TableManager.Entity;
using MyAddressManageTool.TableManager.Mapper;
using MyAddressManageTool.Core.ExceptionManage;
using MyAddressManageTool.Core.Message;
using MyAddressManageTool.Core.Message.Implement;

namespace MyAddressManageTool.MyApi
{
    internal class NumberControllManager
    {
        public static string GetNextNumber(OleDbTransaction transaction, string numberId)
        {
            // _/_/_/_/_/_/_/_/_/_/ 引数チェック _/_/_/_/_/_/_/_/_/_/
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }

            if (string.IsNullOrEmpty(numberId))
            {
                throw new ArgumentNullException(nameof(numberId));
            }

            // _/_/_/_/_/_/_/_/_/_/ 主処理 _/_/_/_/_/_/_/_/_/_/
            TableManagerControll<NumberControllTableEntity> tableManager = new(new NumberControlTableMapper(), transaction);
            
            // テーブルマネージャー引数構成
            NumberControllTableEntity inputEntity = new();
            inputEntity.Id = numberId;

            // データ取得
            NumberControllTableEntity updateEntity = tableManager.FindByPrimarykey(inputEntity);

            if (updateEntity == null) throw new NotFoundException($"{numberId}(NuberControll)");

            // 番号取得
            int currentNumber = updateEntity.CurrentNumber;
            int nextNumber = currentNumber + 1;
            int maxNumber = updateEntity.MaxNumber;

            // 最大値チェック
            if (nextNumber > maxNumber)
            {
                IMessageManager message = new ErrorMessageManagerImpl();
                throw new ApplicationException(message.GetMessage("ES0003", new string[] { numberId }));
            }

            // 採番テーブル更新
            updateEntity.CurrentNumber = nextNumber;
            tableManager.Update(updateEntity);

            // 採番値生成
            string prefixNumber = nextNumber.ToString();

            if (!string.IsNullOrEmpty(updateEntity.PreFix))
            {
                prefixNumber = prefixNumber.PadLeft(totalWidth: maxNumber.ToString().Length, '0');
                prefixNumber = updateEntity.PreFix + prefixNumber;
            }

            return prefixNumber;
        } 
    }
}
