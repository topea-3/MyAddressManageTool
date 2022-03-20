using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MyAddressManageTool.View.HostInformationView
{
    public class HostInformation : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private string? _hostId;
        public string? HostId
        { 
            get => _hostId;
            set
            {
                if (_hostId != value)
                {
                    _hostId = value;
                    OnPropertyChanged(nameof(HostId));
                }
            }
        }
        private int _seqNo  = 1;
        public int SeqNo
        {
            get => _seqNo;
            set
            {
                if (_seqNo != value)
                {
                    _seqNo = value;
                    OnPropertyChanged(nameof(SeqNo));
                }
            }
        }
        private string? _hostName;
        public string? HostName
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
        private string? _subName1;
        public string? SubName1
        {
            get => _subName1;
            set
            {
                if (_subName1 != value)
                {
                    _subName1 = value;
                    OnPropertyChanged(nameof(SubName1));
                }
            }
        }
        private string? _subName2;
        public string? SubName2
        {
            get => _subName2;
            set
            {
                if (_subName2 != value)
                {
                    _subName2 = value;
                    OnPropertyChanged(nameof(SubName2));
                }
            }
        }
        private string? _subName3;

        public string? SubName3
        {
            get => _subName3;
            set
            {
                if (_subName3 != value)
                {
                    _subName3 = value;
                    OnPropertyChanged(nameof(SubName3));
                }
            }
        }
        private string? _subName4;
        public string? SubName4
        {
            get => _subName4;
            set
            {
                if (_subName4 != value)
                {
                    _subName4 = value;
                    OnPropertyChanged(nameof(SubName4));
                }
            }
        }
        private string? _subName5;
        public string? SubName5
        {
            get => _subName5;
            set
            {
                if (_subName5 != value)
                {
                    _subName5 = value;
                    OnPropertyChanged(nameof(SubName5));
                }
            }
        }
        private string? _addressNumber1;
        public string? AddressNumber1
        {
            get => _addressNumber1;
            set
            {
                if (_addressNumber1 != value)
                {
                    _addressNumber1 = value;
                    OnPropertyChanged(nameof(AddressNumber1));
                }
            }
        }
        private string? _addressNumber2;
        public string? AddressNumber2
        {
            get => _addressNumber2;
            set
            {
                if (_addressNumber2 != value)
                {
                    _addressNumber2 = value;
                    OnPropertyChanged(nameof(AddressNumber2));
                }
            }
        }
        private string? _address1;
        public string? Address1
        {
            get => _address1;
            set
            {
                if (_address1 != value)
                {
                    _address1 = value;
                    OnPropertyChanged(nameof(Address1));
                }
            }
        }
        private string? _address2;
        public string? Address2
        {
            get => _address2;
            set
            {
                if (_address2 != value)
                {
                    _address2 = value;
                    OnPropertyChanged(nameof(Address2));
                }
            }
        }
        private string? _remarks;
        public string? Remarks
        {
            get => _remarks;
            set
            {
                if (_remarks != value)
                {
                    _remarks = value;
                    OnPropertyChanged(nameof(Remarks));
                }
            }
        }
        private string? _deleteFlag;
        public string? DeleteFlag
        {
            get => _deleteFlag;
            set
            {
                if (_deleteFlag != value)
                {
                    _deleteFlag = value;
                    OnPropertyChanged(nameof(DeleteFlag));
                }
            }
        }
        private DateTime? _createDateTime;
        public DateTime? CreateDateTime
        {
            get => _createDateTime;
            set
            {
                if (_createDateTime != value)
                {
                    _createDateTime = value;
                    OnPropertyChanged(nameof(CreateDateTime));
                }
            }
        }
        private DateTime? _updateDateTime;

        public DateTime? UpdateDateTime
        {
            get => _updateDateTime;
            set
            {
                if (_updateDateTime != value)
                {
                    _updateDateTime = value;
                    OnPropertyChanged(nameof(UpdateDateTime));
                }
            }
        }
    }
}
