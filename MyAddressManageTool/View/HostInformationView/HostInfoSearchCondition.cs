using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MyAddressManageTool.View.HostInformationView
{
    /// <summary>
    /// HostInformationInquiryPageの検索条件設定クラス
    /// </summary>
    internal class HostInfoSearchCondition : INotifyPropertyChanged
    {
        // 変更通知プロパティ
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
            =>  PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        // 履歴表示フラグ
        private bool _isHistoryView = false;
        public bool IsHistoryView
        { 
            get => this._isHistoryView;
            set
            {
                if (this._isHistoryView != value)
                {
                    this._isHistoryView = value;
                    OnPropertyChanged(nameof(IsHistoryView));
                }
            }
        }
        //ホスト名称
        private string? _hostName;
        public string?  HostName
        {
            get => _hostName;
            set
            {
                if (_hostName != value)
                {
                    _hostName = value;
                    OnPropertyChanged(nameof(HostName));
                }
            }
        }
        // 検索結果件数
        private string _dataCount = "検索結果：0 件";
        public string DataCount 
        {
            get => _dataCount;
            set
            {
                if (_dataCount != value)
                {
                    _dataCount = $"検索結果：{value} 件";
                    OnPropertyChanged(nameof(DataCount));
                }
            }
        }
        // 検索日時
        private string _searchDateTime = DateTime.Now.ToString();
        public string SearchDateTime
        { 
            get => _searchDateTime;
            set
            {
                if (!_searchDateTime.Equals(value))
                {
                    _searchDateTime = value;
                    OnPropertyChanged(nameof(SearchDateTime));
                }
            }
        }
        // 検索結果
        private IList<Inquiry4HostInformation> results = new List<Inquiry4HostInformation>();
        public IList<Inquiry4HostInformation> Results
        {
            get => results;
            set
            {
                results = value;
                OnPropertyChanged(nameof(Results));
            }
        }
    }
}
