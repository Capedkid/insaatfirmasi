using System.ComponentModel.DataAnnotations;

namespace InsaatFirmasi.Models
{
    public class ReferenceLogo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Logo görsel yolu zorunludur")]
        [StringLength(200, ErrorMessage = "Logo yolu en fazla 200 karakter olabilir")]
        public string ImagePath { get; set; } = string.Empty;

        public int SortOrder { get; set; } = 0;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}


