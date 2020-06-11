using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace WolfyJudgement
{
    public class Cases
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public decimal? Amount { get; set; }
        public string CourtType { get; set; }
        public string CaseType { get; set; }
        public DateTime? FillingDate { get; set; }
        public string Judge { get; set; }
        public string DocketType { get; set; }
        public string Description { get; set; }
        public string CaseNo { get; set; }
        public string CaseUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Color ViewCellBackgroundColor { get; internal set; }
    }

    public class CaseData
    {
        public long TotalCount;

        public List<Cases> cases;
    }
}
