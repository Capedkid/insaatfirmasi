using System.ComponentModel.DataAnnotations;

namespace InsaatFirmasi.Models
{
    public class ContactInfo
    {
        public int Id { get; set; } = 1; // Tek kayıt olacak

        [StringLength(100, ErrorMessage = "Telefon en fazla 100 karakter olabilir")]
        public string? Phone { get; set; }

        [StringLength(100, ErrorMessage = "E-posta en fazla 100 karakter olabilir")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
        public string? Email { get; set; }

        [StringLength(500, ErrorMessage = "Adres en fazla 500 karakter olabilir")]
        public string? Address { get; set; }

        [StringLength(50, ErrorMessage = "Enlem en fazla 50 karakter olabilir")]
        public string? Latitude { get; set; }

        [StringLength(50, ErrorMessage = "Boylam en fazla 50 karakter olabilir")]
        public string? Longitude { get; set; }

        [StringLength(100, ErrorMessage = "WhatsApp numarası en fazla 100 karakter olabilir")]
        public string? WhatsApp { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}

