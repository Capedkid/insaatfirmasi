using System.ComponentModel.DataAnnotations;

namespace InsaatFirmasi.Models
{
    public class ContactMessage
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad soyad zorunludur")]
        [StringLength(100, ErrorMessage = "Ad soyad en fazla 100 karakter olabilir")]
        public string FullName { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Ad soyad en fazla 100 karakter olabilir")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "E-posta zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
        [StringLength(100, ErrorMessage = "E-posta en fazla 100 karakter olabilir")]
        public string Email { get; set; } = string.Empty;

        [StringLength(20, ErrorMessage = "Telefon en fazla 20 karakter olabilir")]
        public string? Phone { get; set; }

        [StringLength(100, ErrorMessage = "Şirket en fazla 100 karakter olabilir")]
        public string? Company { get; set; }

        [StringLength(50, ErrorMessage = "IP adresi en fazla 50 karakter olabilir")]
        public string? IpAddress { get; set; }

        [StringLength(100, ErrorMessage = "Konu en fazla 100 karakter olabilir")]
        public string? Subject { get; set; }

        [Required(ErrorMessage = "Mesaj zorunludur")]
        [StringLength(1000, ErrorMessage = "Mesaj en fazla 1000 karakter olabilir")]
        public string Message { get; set; } = string.Empty;

        public bool IsRead { get; set; } = false;

        public bool IsReplied { get; set; } = false;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? ReadDate { get; set; }

        public DateTime? ReplyDate { get; set; }
    }
}
