using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAddressManageTool.View.AddressInfoView
{
    internal class Inquiry4AddressInformation
    {
        // 住所ID
        public string? AddressId { get; set; }
        // 連番
        public int SeqNo { get; set; }
        // 氏名
        public string? Name { get; set; }
        // 住所
        public string? Address { get; set; }
        // 担当ホスト名称
        public string? BelongHostName { get; set; }
        // 年賀状作成対象
        public string? NyLetterCreateFlag { get; set; }
        // メモ
        public string? Remarks { get; set; }
        // 削除フラグ
        public string? DeleteFlag { get; set; }
    }
}
