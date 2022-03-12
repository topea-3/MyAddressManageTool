using MyAddressManageTool.Core.Message;
using MyAddressManageTool.Core.Message.Implement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyAddressManageTool.Core.ViewModel.Implement
{
    /// <summary>
    /// 画面バリデーションオブジェクト
    /// </summary>
    internal class MyValidation
    {
        const string REQUIRED_CHECK_ERROR_ID = "EV0001";
        const string MIN_LENGTH_CHECK_ERROR_ID = "EV0002";
        const string MAX_LENGTH_CHECK_ERROR_ID = "EV0003";
        const string FORMAT_CHECK_ERROR = "EV0004";
        // Validationエラー情報リスト
        private readonly List<ValidationError> validationErrors = new();

        /// <summary>
        /// バリデーションエラー結果取得。
        /// エラーなしの場合空のListを返却する。
        /// </summary>
        /// <returns>結果</returns>
        public IList<string> GetResults()
        {
            IList<string> results = new List<string>();

            foreach (var error in validationErrors)
            {
                results.Add(error.GetMessage());
            }

            return results;
        }

        /// <summary>
        /// validation実行.
        /// Validation.resx：[画面ID]-[項目名]=チェック名_引数1_引数2_…__チェック名_引数1_引数2_…
        /// </summary>
        /// <param name="context"></param>
        public void ExecuteValidate<T>(string viewId, T viewItems)
        {
            if (viewId == null)
            {
                throw new ArgumentNullException(nameof(viewId));
            }

            if (viewItems == null)
            {
                throw new ArgumentNullException(nameof(viewItems));
            }

            Type viewItemsType = viewItems.GetType();

            foreach (PropertyInfo propertyInfo in viewItemsType.GetProperties())
            {
                string validateKey = MakeValidationKey(viewId, propertyInfo.Name);
                string? viewItemName = VIewItemNameMap.ResourceManager.GetString(validateKey);
                string[]? validateRule = Validation.ResourceManager.GetString(validateKey)?.Split("__");
                object? itemValue = propertyInfo.GetValue(viewItems);

                if (validateRule == null)
                {
                    continue;
                }

                foreach (string rule in validateRule)
                {
                    InnerValidate(itemValue, viewItemName, rule);
                }

            }
        }

        /// <summary>
        /// バリデーションルール解析実行.
        /// バリデーションルール：チェック名_引数1_引数2_…
        /// </summary>
        /// <param name="itemValue">項目値</param>
        /// <param name="itemName">項目名</param>
        /// <param name="validateRule">バリデーションルール</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        private void InnerValidate(object? itemValue, string? itemName, string validateRule)
        {
            // _/_/_/_/_/_/_/_/_/_/_/_/ 引数チェック _/_/_/_/_/_/_/_/_/_/_/_/
            if (string.IsNullOrEmpty(itemName))
            {
                throw new ArgumentException(itemName, nameof(itemName));
            }

            if (string.IsNullOrEmpty(validateRule))
            {
                throw new ArgumentException(null, nameof(validateRule));
            }

            // _/_/_/_/_/_/_/_/_/_/_/_/ ルール解析 _/_/_/_/_/_/_/_/_/_/_/_/
            string[] rule = validateRule.Split('_');

            // _/_/_/_/_/_/_/_/_/_/_/_/ バリデーション実行 _/_/_/_/_/_/_/_/_/_/_/_/
            if ("RequiredCheck".Equals(rule[0], StringComparison.Ordinal))
            {
                // 必須チェック
                RequiredCheck(itemValue, itemName);
            }

            if ("StringLengthCheck".Equals(rule[0], StringComparison.Ordinal))
            {
                // 文字列長チェック
                StringLengthCheck(itemValue, itemName, rule[1], rule[2]);
            }

            if ("FormatCheck".Equals(rule[0], StringComparison.Ordinal))
            {
                // 正規表現フォーマットチェック
                FormatCheck(itemValue, itemName, rule[1]);
            }
        }

        /// <summary>
        /// バリデーション必須チェック。
        /// RequiredCheck
        /// </summary>
        /// <param name="itemValue">項目値</param>
        /// <param name="itemName">項目名</param>
        private void RequiredCheck(object? itemValue, string itemName)
        {
            if (itemValue == null)
            {
                validationErrors.Add(new ValidationError(itemName, REQUIRED_CHECK_ERROR_ID, new List<string>()));
                return;
            }

            if (typeof(string) == itemValue.GetType())
            {
                if (string.IsNullOrEmpty((string)itemValue))
                {
                    validationErrors.Add(new ValidationError(itemName, REQUIRED_CHECK_ERROR_ID, new List<string>()));
                }
            }
            else
            {
                if (null == itemValue)
                {
                    validationErrors.Add(new ValidationError(itemName, REQUIRED_CHECK_ERROR_ID, new List<string>()));
                }
            }
        }

        /// <summary>
        /// バリエーション文字列長チェック。
        /// StringLengthCheck_最大長_最小長
        /// </summary>
        /// <param name="itemValue">項目値</param>
        /// <param name="itemName">項目名</param>
        /// <param name="maxLengthStr">最大長</param>
        /// <param name="minLengthStr">最小長</param>
        private void StringLengthCheck(object? itemValue, string itemName, string maxLengthStr, string minLengthStr)
        {
            // 未設定の場合はチェックしない
            if (IsNullOrEmptyObject(itemValue))
            {
                return;
            }

            // 最小長チェック
            if (!string.IsNullOrEmpty(minLengthStr))
            {
                if (int.Parse(minLengthStr) > ((string) itemValue).Length)
                {
                    List<string> parameterList = new();
                    parameterList.Add(minLengthStr);
                    validationErrors.Add(new ValidationError(itemName, MIN_LENGTH_CHECK_ERROR_ID, parameterList));
                }
            }

            // 最大長チェック
            if (!string.IsNullOrEmpty(maxLengthStr))
            {
                if (int.Parse(maxLengthStr) < ((string)itemValue).Length)
                {
                    List<string> parameterList = new();
                    parameterList.Add(maxLengthStr);
                    validationErrors.Add(new ValidationError(itemName, MAX_LENGTH_CHECK_ERROR_ID, parameterList));
                }
            }
        }

        /// <summary>
        /// 正規表現フォーマットチェック
        /// FormatCheck_正規表現
        /// </summary>
        /// <param name="itemValue">値</param>
        /// <param name="itemName">項目名</param>
        /// <param name="format">正規表現フォーマット</param>
        private void FormatCheck(object? itemValue, string itemName, string format)
        {
            // 未設定の場合はチェックしない
            if (IsNullOrEmptyObject(itemValue))
            {
                return;
            }

            string itemValueStr = (string)itemValue;

            if (string.IsNullOrEmpty(format))
            {
                return;
            }

            if (!Regex.IsMatch(itemValueStr, format))
            {
                validationErrors.Add(new ValidationError(itemName, FORMAT_CHECK_ERROR, new List<string>()));
            }
        }

        /// <summary>
        /// Objectの未設定判定
        /// </summary>
        /// <param name="arg">チェック対象</param>
        /// <returns>bool</returns>
        private static bool IsNullOrEmptyObject(object? arg)
        {
            if (arg == null)
            {
                return true;
            }

            if (typeof(string) == arg.GetType())
            {
                return string.IsNullOrEmpty((string)arg);
            }

            return false;
        }

        /// <summary>
        /// validation.resxに対応するキーを構成する
        /// </summary>
        /// <param name="viewId">画面ID</param>
        /// <param name="itemName">項目物理名</param>
        /// <returns>バリデーションキー</returns>
        private static string MakeValidationKey(string viewId, string itemName)
        {
            return viewId + "-" + itemName;
        }

        /// <summary>
        /// バリデーションエラー情報
        /// </summary>
        internal class ValidationError
        {
            // メッセージマネージャー
            private readonly IMessageManager messageManager = new ErrorMessageManagerImpl();


            private ValidationError() 
            {
                //  ダミー実装
                ItemName = "";
                ErrorId = "";
                ParameterList = new List<string>();
            }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="itemName">項目名</param>
            /// <param name="errorId">エラーID</param>
            /// <param name="parameterList">その他パラメータ</param>
            public ValidationError(string itemName, string errorId, List<string> parameterList)
            {
                ItemName = itemName;
                ErrorId = errorId;
                ParameterList = parameterList;
            }

            // 項目名
            public string ItemName { get; }

            // エラーID
            private string ErrorId { get; set; }

            // その他パラメータ
            private List<string> ParameterList { get; set; }

            /// <summary>
            /// メッセージ取得
            /// </summary>
            /// <returns>エラーメッセージ</returns>
            public string GetMessage()
            {
                List<string> messageParameterList = new();
                messageParameterList.Add(ItemName);
                messageParameterList.AddRange(ParameterList);

                return messageManager.GetMessage(this.ErrorId, messageParameterList.ToArray());
            }
        }
    }
}
