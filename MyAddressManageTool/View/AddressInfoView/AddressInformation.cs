using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MyAddressManageTool.View.AddressInfoView
{
    internal class AddressInformation : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public string? _addressId;
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

        public int _seqNo = 1;
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
        
        public string? _familyName;
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

        public string? _name;
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

        public string? _honorificTitle;
        public string? HonorificTitle
        {
            get => _honorificTitle;
            set
            {
                if (_honorificTitle != value)
                {
                    _honorificTitle = value;
                    OnPropertyChanged(nameof(HonorificTitle));
                }
            }
        }

        public string? _subName1;
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

        public string? _subName1Honor;
        public string? SubName1Honor
        {
            get => _subName1Honor;
            set
            {
                if (_subName1Honor != value)
                {
                    _subName1Honor = value;
                    OnPropertyChanged(nameof(SubName1Honor));
                }
            }
        }

        public string? _subName2;
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

        public string? _subName2Honor;
        public string? SubName2Honor
        {
            get => _subName2Honor;
            set
            {
                if (_subName2Honor != value)
                {
                    _subName2Honor = value;
                    OnPropertyChanged(nameof(SubName2Honor));
                }
            }
        }

        public string? _subName3;
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

        public string? _subName3Honor;
        public string? SubName3Honor
        {
            get => _subName3Honor;
            set
            {
                if (_subName3Honor != value)
                {
                    _subName3Honor = value;
                    OnPropertyChanged(nameof(SubName3Honor));
                }
            }
        }

        public string? _subName4;
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

        public string? _subName4Honor;
        public string? SubName4Honor
        {
            get => _subName4Honor;
            set
            {
                if (_subName4Honor != value)
                {
                    _subName4Honor = value;
                    OnPropertyChanged(nameof(SubName4Honor));
                }
            }
        }

        public string? _subName5;
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

        public string? _subName5Honor;
        public string? SubName5Honor
        {
            get => _subName5Honor;
            set
            {
                if (_subName5Honor != value)
                {
                    _subName5Honor = value;
                    OnPropertyChanged(nameof(SubName5Honor));
                }
            }
        }

        public string? _addressNumber1;
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

        public string? _addressNumber2;
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

        public string? _address1;
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

        public string? _address2;
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

        public string? _belongHostId;
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

        public string? _nyLetterCreateFlag;
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

        public string? _remarks;
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

        public string? _deleteFlag;
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

        public DateTime? _createDateTime;
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

        public DateTime? _updateDateTime;

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
