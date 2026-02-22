using System;

namespace DekelApp.Models
{
    public class GeneralInfoModel
    {
        public string? UnitName { get; set; }
        public string? OperationName { get; set; }
        public DateTime? Date { get; set; } = DateTime.Now;
        public string? GeneralNotes { get; set; }
    }
}
