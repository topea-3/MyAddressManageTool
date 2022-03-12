using MyAddressManageTool.Core.ExceptionManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAddressManageTool.Core.Message.Implement
{
    /// <summary>
    /// メッセージ管理実装クラス
    /// </summary>
    internal class MessageManagerImpl : IMessageManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="parameterArray"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        public string GetMessage(string id, string[] parameterArray)
        {

            // パラメータチェック
            if (id == null)
            {
                IMessageManager messageManager = new ErrorMessageManagerImpl();
                throw new ArgumentNullException(messageManager.GetMessage("ES0002", new string[] { "id" }));
            }

            // メッセージ取得
            string? message = Message.ResourceManager.GetString(name: id);

            // メッセージ取得失敗判定
            if (message == null)
            {
                throw new NotFoundException("message");
            }

            // メッセージへのプレースフォルダー値設定
            int index = 0;

            foreach (string parameter in parameterArray)
            {
                message = message.Replace($"{{{index++}}}", parameter);
            }
            
            // 返却
            return message;
        }
    }
}
