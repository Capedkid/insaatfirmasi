using System.ComponentModel.DataAnnotations;

namespace InsaatFirmasi.Models
{
    public class Catalog
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Katalog adı zorunludur")]
        [StringLength(200, ErrorMessage = "Katalog adı en fazla 200 karakter olabilir")]
        public string Name { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Katalog başlığı en fazla 200 karakter olabilir")]
        public string? Title { get; set; }

        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir")]
        public string? Description { get; set; }

        [StringLength(200, ErrorMessage = "Dosya yolu en fazla 200 karakter olabilir")]
        public string FilePath { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Dosya URL'i en fazla 200 karakter olabilir")]
        public string? FileUrl { get; set; }

        [StringLength(50, ErrorMessage = "Dosya boyutu en fazla 50 karakter olabilir")]
        public string? FileSize { get; set; }

        [StringLength(10, ErrorMessage = "Dosya uzantısı en fazla 10 karakter olabilir")]
        public string? FileExtension { get; set; }

        [StringLength(200, ErrorMessage = "Kapak resmi yolu en fazla 200 karakter olabilir")]
        public string? CoverImagePath { get; set; }

        [StringLength(200, ErrorMessage = "Kapak resmi URL'i en fazla 200 karakter olabilir")]
        public string? CoverImageUrl { get; set; }

        public bool IsActive { get; set; } = true;

        public int DownloadCount { get; set; } = 0;

        public long FileSizeBytes { get; set; } = 0;

        public int SortOrder { get; set; } = 0;

        [StringLength(100, ErrorMessage = "SEO başlığı en fazla 100 karakter olabilir")]
        public string? SeoTitle { get; set; }

        [StringLength(200, ErrorMessage = "SEO açıklaması en fazla 200 karakter olabilir")]
        public string? SeoDescription { get; set; }

        [StringLength(100, ErrorMessage = "SEO anahtar kelimeleri en fazla 100 karakter olabilir")]
        public string? SeoKeywords { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? UpdatedDate { get; set; }
    }
}
