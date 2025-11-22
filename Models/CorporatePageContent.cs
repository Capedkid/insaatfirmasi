using System.ComponentModel.DataAnnotations;

namespace InsaatFirmasi.Models
{
    public class CorporatePageContent
    {
        public int Id { get; set; } = 1; // Tek kayıt

        // Tarihçe metni
        [StringLength(200)]
        public string? HistoryTitle { get; set; }

        [StringLength(800)]
        public string? HistoryText1 { get; set; }

        [StringLength(800)]
        public string? HistoryText2 { get; set; }

        // Tarihçemiz yanındaki 4 kart
        [StringLength(150)]
        public string? HistoryItem1Title { get; set; }

        [StringLength(300)]
        public string? HistoryItem1Text { get; set; }

        [StringLength(150)]
        public string? HistoryItem2Title { get; set; }

        [StringLength(300)]
        public string? HistoryItem2Text { get; set; }

        [StringLength(150)]
        public string? HistoryItem3Title { get; set; }

        [StringLength(300)]
        public string? HistoryItem3Text { get; set; }

        [StringLength(150)]
        public string? HistoryItem4Title { get; set; }

        [StringLength(300)]
        public string? HistoryItem4Text { get; set; }

        // Misyon / Vizyon
        [StringLength(200)]
        public string? MissionTitle { get; set; }

        [StringLength(800)]
        public string? MissionText { get; set; }

        [StringLength(200)]
        public string? VisionTitle { get; set; }

        [StringLength(800)]
        public string? VisionText { get; set; }

        // Değerler bölümü
        [StringLength(200)]
        public string? ValuesTitle { get; set; }

        [StringLength(500)]
        public string? ValuesSubtitle { get; set; }

        [StringLength(150)]
        public string? Value1Title { get; set; }

        [StringLength(300)]
        public string? Value1Text { get; set; }

        [StringLength(150)]
        public string? Value2Title { get; set; }

        [StringLength(300)]
        public string? Value2Text { get; set; }

        [StringLength(150)]
        public string? Value3Title { get; set; }

        [StringLength(300)]
        public string? Value3Text { get; set; }

        [StringLength(150)]
        public string? Value4Title { get; set; }

        [StringLength(300)]
        public string? Value4Text { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}


