using System.ComponentModel.DataAnnotations;

namespace InsaatFirmasi.Models
{
    public class Slider
    {
        public int Id { get; set; }

        [StringLength(100, ErrorMessage = "Üst başlık en fazla 100 karakter olabilir")]
        public string? Kicker { get; set; }

        [Required(ErrorMessage = "Başlık zorunludur")]
        [StringLength(150, ErrorMessage = "Başlık en fazla 150 karakter olabilir")]
        public string Title { get; set; } = string.Empty;

        [StringLength(250, ErrorMessage = "Alt başlık en fazla 250 karakter olabilir")]
        public string? Subtitle { get; set; }

        [StringLength(200, ErrorMessage = "Görsel yolu en fazla 200 karakter olabilir")]
        public string? ImagePath { get; set; }

        [StringLength(300, ErrorMessage = "Bağlantı URL'i en fazla 300 karakter olabilir")]
        public string? LinkUrl { get; set; }

        public bool IsActive { get; set; } = true;

        public int SortOrder { get; set; } = 0;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? UpdatedDate { get; set; }
    }
}


