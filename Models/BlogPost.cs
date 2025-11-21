using System.ComponentModel.DataAnnotations;

namespace InsaatFirmasi.Models
{
    public class BlogPost
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlık zorunludur")]
        [StringLength(200, ErrorMessage = "Başlık en fazla 200 karakter olabilir")]
        public string Title { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Özet en fazla 500 karakter olabilir")]
        public string? Summary { get; set; }

        [Required(ErrorMessage = "İçerik zorunludur")]
        public string Content { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Resim yolu en fazla 200 karakter olabilir")]
        public string? ImagePath { get; set; }

        [StringLength(200, ErrorMessage = "Resim URL'i en fazla 200 karakter olabilir")]
        public string? ImageUrl { get; set; }

        [StringLength(100, ErrorMessage = "Etiketler en fazla 100 karakter olabilir")]
        public string? Tags { get; set; }

        [StringLength(100, ErrorMessage = "SEO başlığı en fazla 100 karakter olabilir")]
        public string? SeoTitle { get; set; }

        [StringLength(200, ErrorMessage = "SEO açıklaması en fazla 200 karakter olabilir")]
        public string? SeoDescription { get; set; }

        [StringLength(100, ErrorMessage = "SEO anahtar kelimeleri en fazla 100 karakter olabilir")]
        public string? SeoKeywords { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsPublished { get; set; } = false;

        public bool IsFeatured { get; set; } = false;

        [StringLength(100, ErrorMessage = "Kategori en fazla 100 karakter olabilir")]
        public string? Category { get; set; }

        [StringLength(100, ErrorMessage = "Yazar en fazla 100 karakter olabilir")]
        public string? Author { get; set; }

        public int ReadTime { get; set; } = 0;

        public int ViewCount { get; set; } = 0;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? UpdatedDate { get; set; }

        public DateTime? PublishedDate { get; set; }
    }
}
