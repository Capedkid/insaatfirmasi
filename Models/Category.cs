using System.ComponentModel.DataAnnotations;

namespace InsaatFirmasi.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kategori adı zorunludur")]
        [StringLength(100, ErrorMessage = "Kategori adı en fazla 100 karakter olabilir")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir")]
        public string? Description { get; set; }

        [StringLength(200, ErrorMessage = "Resim yolu en fazla 200 karakter olabilir")]
        public string? ImagePath { get; set; }

        [StringLength(200, ErrorMessage = "Resim URL'i en fazla 200 karakter olabilir")]
        public string? ImageUrl { get; set; }

        public int SortOrder { get; set; } = 0;

        [StringLength(100, ErrorMessage = "SEO başlığı en fazla 100 karakter olabilir")]
        public string? SeoTitle { get; set; }

        [StringLength(200, ErrorMessage = "SEO açıklaması en fazla 200 karakter olabilir")]
        public string? SeoDescription { get; set; }

        [StringLength(100, ErrorMessage = "SEO anahtar kelimeleri en fazla 100 karakter olabilir")]
        public string? SeoKeywords { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? UpdatedDate { get; set; }

        // Navigation Properties
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
