using MyAddressManageTool.MyApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAddressManageTool.View.HostInformationView
{
    internal class Inquiry4HostInformation
    {
        public string? HostId { get; set; }
        public int SeqNo { get; set; }
        public string? HostName { get; set; }
        public string? FamilyName { get; set; }
        public string? Name { get; set; }
        public string? Remarks { get; set; }
        // 削除フラグ
        public string? DeleteFlag { get; set; }
    }
}
