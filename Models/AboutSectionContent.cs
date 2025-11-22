using System.ComponentModel.DataAnnotations;

namespace InsaatFirmasi.Models
{
    public class AboutSectionContent
    {
        public int Id { get; set; } = 1; // Tek kayıt

        [StringLength(200)]
        public string? Title { get; set; }

        [StringLength(500)]
        public string? Subtitle { get; set; }

        [StringLength(150)]
        public string? Item1Title { get; set; }

        [StringLength(300)]
        public string? Item1Text { get; set; }

        [StringLength(150)]
        public string? Item2Title { get; set; }

        [StringLength(300)]
        public string? Item2Text { get; set; }

        [StringLength(150)]
        public string? Item3Title { get; set; }

        [StringLength(300)]
        public string? Item3Text { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}


