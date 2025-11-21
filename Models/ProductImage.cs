using System.ComponentModel.DataAnnotations;

namespace InsaatFirmasi.Models
{
    public class ProductImage
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Resim yolu zorunludur")]
        [StringLength(200, ErrorMessage = "Resim yolu en fazla 200 karakter olabilir")]
        public string ImagePath { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Resim URL'i en fazla 200 karakter olabilir")]
        public string? ImageUrl { get; set; }

        [StringLength(100, ErrorMessage = "Alt text en fazla 100 karakter olabilir")]
        public string? AltText { get; set; }

        public bool IsMainImage { get; set; } = false;

        public bool IsMain { get; set; } = false;

        public int SortOrder { get; set; } = 0;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Foreign Key
        [Required]
        public int ProductId { get; set; }

        // Navigation Properties
        public virtual Product Product { get; set; } = null!;
    }
}
