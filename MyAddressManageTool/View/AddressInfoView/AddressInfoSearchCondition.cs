using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MyAddressManageTool.View.AddressInfoView
{
    internal class AddressInfoSearchCondition : INotifyPropertyChanged
    {
        // 変更通知プロパティ
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

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

        // 住所ID
        private string? _addressId;
        public string? AddressId
        {
            get => _addressId;
            set
            {
                if (_addressId != value)
                {
                    _addressId = value;
                    OnPropertyChanged(nameof(AddressId));
                }
            }
        }

        // 姓
        private string? _familyName;
        public string? FamilyName
        {
            get => _familyName;
            set
            {
                if (_familyName != value)
                {
                    _familyName = value;
                    OnPropertyChanged(nameof(FamilyName));
                }
            }
        }

        // 名
        private string? _name;
        public string? Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        // 連名
        private string? _subName;
        public string? SubName
        {
            get => _subName;
            set
            {
                if (_subName != value)
                {
                    _subName = value;
                    OnPropertyChanged(nameof(SubName));
                }
            }
        }

        // 住所
        private string? _address;
        public string? Address
        {
            get => _address;
            set
            {
                if (_address != value)
                {
                    _address = value;
                    OnPropertyChanged(nameof(Address));
                }
            }
        }

        // 担当ホストID
        private string? _belongHostId;
        public string? BelongHostId
        {
            get => _belongHostId;
            set
            {
                if (_belongHostId != value)
                {
                    _belongHostId = value;
                    OnPropertyChanged(nameof(BelongHostId));
                }
            }
        }

        // 担当ホスト名称
        private string? _belongHostName;
        public string? BelongHostName
        {
            get => _belongHostName;
            set
            {
                if (_belongHostName != value)
                {
                    _belongHostName = value;
                    OnPropertyChanged(nameof(BelongHostName));
                }
            }
        }

        // 年賀状作成対象
        private string? _nyLetterCreateFlag;
        public string? NyLetterCreateFlag
        {
            get => _nyLetterCreateFlag;
            set
            {
                if (_nyLetterCreateFlag != value)
                {
                    _nyLetterCreateFlag = value;
                    OnPropertyChanged(nameof(NyLetterCreateFlag));
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
        private IList<Inquiry4AddressInformation> results = new List<Inquiry4AddressInformation>();
        public IList<Inquiry4AddressInformation> Results
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
