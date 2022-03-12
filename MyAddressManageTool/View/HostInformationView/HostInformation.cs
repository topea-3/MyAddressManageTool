using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAddressManageTool.View.HostInformationView
{
    internal class HostInformation
    {
        public string? HostId { get; set; }
        public int SeqNo { get; set; }
        public string? HostName { get; set; }
        public string? FamilyName { get; set; }
        public string? Name { get; set; }
        public string? SubName1 { get; set; }
        public string? SubName2 { get; set; }
        public string? SubName3 { get; set; }
        public string? SubName4 { get; set; }
        public string? SubName5 { get; set; }
        public string? AddressNumber1 { get; set; }
        public string? AddressNumber2 { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? Remarks { get; set; }
        public string? DeleteFlag { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
