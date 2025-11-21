using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsaatFirmasi.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ürün adı zorunludur")]
        [StringLength(200, ErrorMessage = "Ürün adı en fazla 200 karakter olabilir")]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Açıklama en fazla 1000 karakter olabilir")]
        public string? Description { get; set; }

        [StringLength(2000, ErrorMessage = "Detaylı açıklama en fazla 2000 karakter olabilir")]
        public string? DetailedDescription { get; set; }

        [StringLength(50, ErrorMessage = "Model numarası en fazla 50 karakter olabilir")]
        public string? ModelNumber { get; set; }

        [StringLength(50, ErrorMessage = "Renk en fazla 50 karakter olabilir")]
        public string? Color { get; set; }

        [StringLength(50, ErrorMessage = "Boyut en fazla 50 karakter olabilir")]
        public string? Size { get; set; }

        [StringLength(50, ErrorMessage = "Malzeme en fazla 50 karakter olabilir")]
        public string? Material { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? Price { get; set; }

        [StringLength(200, ErrorMessage = "Ana resim yolu en fazla 200 karakter olabilir")]
        public string? MainImagePath { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsFeatured { get; set; } = false;

        public int SortOrder { get; set; } = 0;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? UpdatedDate { get; set; }

        public int ViewCount { get; set; } = 0;

        [StringLength(1000, ErrorMessage = "Özellikler en fazla 1000 karakter olabilir")]
        public string? Features { get; set; }

        [StringLength(200, ErrorMessage = "Etiketler en fazla 200 karakter olabilir")]
        public string? Tags { get; set; }

        [StringLength(100, ErrorMessage = "SEO başlığı en fazla 100 karakter olabilir")]
        public string? SeoTitle { get; set; }

        [StringLength(200, ErrorMessage = "SEO açıklaması en fazla 200 karakter olabilir")]
        public string? SeoDescription { get; set; }

        [StringLength(100, ErrorMessage = "SEO anahtar kelimeleri en fazla 100 karakter olabilir")]
        public string? SeoKeywords { get; set; }

        // Foreign Key
        public int CategoryId { get; set; }

        // Navigation Properties
        public virtual Category? Category { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
    }
}
