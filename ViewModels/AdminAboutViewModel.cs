using InsaatFirmasi.Models;

namespace InsaatFirmasi.ViewModels
{
    public class AdminAboutViewModel
    {
        public IEnumerable<AboutImage> Images { get; set; } = new List<AboutImage>();
        public AboutSectionContent Content { get; set; } = new AboutSectionContent();
    }
}


