using MyAddressManageTool.Core.Message;
using MyAddressManageTool.Core.Message.Implement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MyAddressManageTool.Core.ExceptionManage
{

    /// <summary>
    /// 対象データなし時の例外クラス
    /// </summary>
    [Serializable()]
    internal class NotFoundException : Exception
    {
        /// <summary>
        /// 固定例外ID
        /// </summary>
        const string ERROR_ID = "ES0001";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="itemName">データなし項目名</param>
        public NotFoundException(string itemName)
            : base(GetMessage(itemName))
        {
            // NOP
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="itemName">データなし項目名</param>
        /// <param name="innerException">原因例外</param>
        public NotFoundException(string itemName, Exception innerException)
            : base(GetMessage(itemName), innerException)
        {
            // NOP
        }

        /// <summary>
        /// 逆シリアル化コンストラクタ
        /// </summary>
        /// <param name="info">シリアル化情報</param>
        /// <param name="context">コンテキスト</param>
        protected NotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // NOP
        }

        /// <summary>
        /// エラーメッセージ取得
        /// </summary>
        /// <param name="itemName">データなし項目名</param>
        /// <returns>例外メッセージ</returns>
        private static string GetMessage(string itemName)
        {
            IMessageManager manager = new ErrorMessageManagerImpl();
            return manager.GetMessage(ERROR_ID, new string[] { itemName });
        }
    }
}
