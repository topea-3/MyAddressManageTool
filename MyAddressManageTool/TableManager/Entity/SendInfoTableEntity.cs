using System;

namespace MyAddressManageTool.TableManager.Entity
{
    internal class SendInfoTableEntity
    {
        public int SendNo { get; set; }
        public DateTime SendDate { get; set; }
        public string? SendCategoryType { get; set; }
        public string? SendHostId { get; set; }
        public string? PartyAddressId { get; set; }
        public string? Remarks { get; set; }
        public string? DeleteFlag { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
