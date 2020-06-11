using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace WolfyJudgement
{
    public class Defendants
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Attorney { get; set; }
        public string Address { get; set; }
        public int? CaseId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Color ViewCellBackgroundColor { get; internal set; }
    }

    public class DefendantData
    {
        public long TotalCount;

        public List<Defendants> defendants;
    }
}
