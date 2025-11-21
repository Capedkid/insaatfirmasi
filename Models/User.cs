using System.ComponentModel.DataAnnotations;

namespace InsaatFirmasi.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı zorunludur")]
        [StringLength(50, ErrorMessage = "Kullanıcı adı en fazla 50 karakter olabilir")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-posta zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
        [StringLength(100, ErrorMessage = "E-posta en fazla 100 karakter olabilir")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifre hash zorunludur")]
        [StringLength(255, ErrorMessage = "Şifre hash en fazla 255 karakter olabilir")]
        public string PasswordHash { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Ad en fazla 100 karakter olabilir")]
        public string? FirstName { get; set; }

        [StringLength(100, ErrorMessage = "Soyad en fazla 100 karakter olabilir")]
        public string? LastName { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsAdmin { get; set; } = false;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? LastLoginDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        // Computed Properties
        public string FullName => $"{FirstName} {LastName}".Trim();
    }
}
