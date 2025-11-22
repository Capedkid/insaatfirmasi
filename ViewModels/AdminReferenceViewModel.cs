using InsaatFirmasi.Models;

namespace InsaatFirmasi.ViewModels
{
    public class AdminReferenceViewModel
    {
        public IEnumerable<ReferenceLogo> Logos { get; set; } = new List<ReferenceLogo>();
        public ReferenceSectionContent Content { get; set; } = new ReferenceSectionContent();
    }
}


