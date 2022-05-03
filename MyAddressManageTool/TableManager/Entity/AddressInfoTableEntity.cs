using System;

namespace MyAddressManageTool.TableManager.Entity
{
    internal class AddressInfoTableEntity
    {
        public string? AddressId { get; set; }
        public int SeqNo { get; set; }
        public string? FamilyName { get; set; }
        public string? Name { get; set; }
        public string? HonorificTitle { get; set; }
        public string? SubName1 { get; set; }
        public string? SubName1Honor { get; set; }
        public string? SubName2 { get; set; }
        public string? SubName2Honor { get; set; }
        public string? SubName3 { get; set; }
        public string? SubName3Honor { get; set; }
        public string? SubName4 { get; set; }
        public string? SubName4Honor { get; set; }
        public string? SubName5 { get; set; }
        public string? SubName5Honor { get; set; }
        public string? AddressNumber1 { get; set; }
        public string? AddressNumber2 { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? BelongHostId { get; set; }
        public string? NyLetterCreateFlag { get; set; }
        public string? Remarks { get; set; }
        public string? DeleteFlag { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
