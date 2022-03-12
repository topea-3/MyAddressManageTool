using System;

namespace MyAddressManageTool.TableManager.Entity
{
    internal class ReceiveInfoTableEntity
    {
        public int ReceiveNo { get; set; }
        public DateTime ReceiveDate { get; set; }
        public string? ReceiveCategoryType { get; set; }
        public string? ReceiveHostId { get; set; }
        public string? PartyAddressId { get; set; }
        public string? Remarks { get; set; }
        public string? DeleteFlag { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
