using MyAddressManageTool.Core.ExceptionManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAddressManageTool.Core.Message.Implement
{
    /// <summary>
    /// エラーメッセージ管理実装クラス
    /// </summary>
    internal class ErrorMessageManagerImpl : IMessageManager
    {
        public string GetMessage(string id, string[] parameterArray)
        {
            // パラメータチェック
            if (id == null)
            {
                IMessageManager messageManager = new ErrorMessageManagerImpl();
                throw new ArgumentNullException(messageManager.GetMessage("ES0002", new string[] { "id" }));
            }

            // メッセージID取得
            string? messageId = ErrorMessageMapping.ResourceManager.GetString(id);

            if (string.IsNullOrEmpty(messageId))
            {
                throw new NotFoundException(nameof(messageId) + $"({messageId})");
            }

            // メッセージ取得
            IMessageManager manager = new MessageManagerImpl();
            return manager.GetMessage(messageId, parameterArray);
        }
    }
}
