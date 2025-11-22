using System.ComponentModel.DataAnnotations;

namespace InsaatFirmasi.Models
{
    public class AboutImage
    {
        public int Id { get; set; }

        [StringLength(200, ErrorMessage = "Görsel yolu en fazla 200 karakter olabilir")]
        public string? ImagePath { get; set; }

        public int SortOrder { get; set; } = 0;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}

