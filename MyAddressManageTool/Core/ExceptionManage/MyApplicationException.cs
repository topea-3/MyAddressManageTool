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
    /// 共通業務例外クラス
    /// </summary>
    [Serializable()]
    internal class MyApplicationException : Exception
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="errorId">エラーID</param>
        /// <param name="parameter">メッセージパラメータ</param>
        public MyApplicationException(string errorId, string[] parameter)
            : base(GetMessage(errorId, parameter))
        {
            // NOP
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="errorId">エラーID</param>
        /// <param name="parameter">メッセージパラメータ</param>
        /// <param name="innerException">原因例外</param>
        public MyApplicationException(string errorId, string[] parameter, Exception innerException)
            : base(GetMessage(errorId, parameter), innerException)
        {
            // NOP
        }

        /// <summary>
        /// 逆シリアル化コンストラクタ
        /// </summary>
        /// <param name="info">シリアル化情報</param>
        /// <param name="context">コンテキスト</param>
        protected MyApplicationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // NOP
        }

        /// <summary>
        /// エラーメッセージ取得
        /// </summary>
        /// <param name="errorId">エラーID</param>
        /// <param name="parameter">メッセージパラメータ</param>
        /// <returns>例外メッセージ</returns>
        private static string GetMessage(string errorId, string[] parameter)
        {
            IMessageManager manager = new ErrorMessageManagerImpl();
            return manager.GetMessage(errorId, parameter);
        }
    }
}
