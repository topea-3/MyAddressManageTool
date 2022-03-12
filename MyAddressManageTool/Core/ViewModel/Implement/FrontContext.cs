using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAddressManageTool.Core.ViewModel.Implement
{
    internal class FrontContext
    {
        // コンテキストキー
        const string KEY_ARG_VIEW_ITEM = "VIEW_ARG_VIEW_ITEM";
        const string KEY_RETURN_VIEW_ITEM = "KEY_RETURN_VIEW_ITEM";

        // コンテキスト
        private Dictionary<string, Object> context = new Dictionary<string, Object>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="viewId">画面ID</param>
        public FrontContext(string viewId)
        {
            ViewID = viewId;
        }

        /// <summary>
        /// 画面IDプロパティ
        /// </summary>
        public string ViewID { get; }

        /// <summary>
        /// 画面入力情報格納
        /// </summary>
        /// <typeparam name="T">画面入力情報の型</typeparam>
        /// <param name="argObject">画面入力情報</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void setViewArgItems<T>(T argObject)
        {
            if (argObject == null)
            {
                throw new ArgumentNullException(nameof(argObject));
            }

            context[KEY_ARG_VIEW_ITEM] = argObject;
        }

        /// <summary>
        /// 画面入力情報の取得
        /// </summary>
        /// <typeparam name="T">画面入力情報の型</typeparam>
        /// <returns>画面入力情報</returns>
        public T getViewArgItems<T>() => (T) context[KEY_ARG_VIEW_ITEM];

        /// <summary>
        /// 画面出力情報格納
        /// </summary>
        /// <typeparam name="T">画面出力情報の型</typeparam>
        /// <param name="returnObject">画面出力情報</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void setViewReturnItems<T>(T returnObject)
        {
            if (null == returnObject)
            {
                throw new ArgumentNullException(nameof(returnObject));
            }
            context[KEY_RETURN_VIEW_ITEM] = returnObject;
        }

        /// <summary>
        /// 画面出力情報の取得
        /// </summary>
        /// <typeparam name="T">画面出力情報の型</typeparam>
        /// <returns>画面出力情報</returns>
        public T getViewReturnItems<T>() => (T) context[KEY_RETURN_VIEW_ITEM];
    }
}
