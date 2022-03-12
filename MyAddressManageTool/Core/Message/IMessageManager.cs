using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAddressManageTool.Core.Message
{
    /// <summary>
    /// メッセージ管理
    /// </summary>
    internal interface IMessageManager
    {
        /// <summary>
        /// メッセージ取得
        /// </summary>
        /// <param name="id">エラーID/メッセージID</param>
        /// <param name="parameterArray">プレースフォルダー設定値</param>
        /// <returns>メッセージ</returns>
        public String GetMessage(string id, string[] parameterArray);
    }
}
