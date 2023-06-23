using Microsoft.Extensions.Options;
using UserInterface.Models;

namespace UserInterface.Helpers
{
    public class PhotoHelpers
    {
        private readonly MicroServiceApiAdjustment _microServiceSettings;

        public PhotoHelpers(IOptions<MicroServiceApiAdjustment> microServiceSettings)
        {
            _microServiceSettings = microServiceSettings.Value;
        }

        public string PhotoUrl(string photoUrl)
        {
            return $"{_microServiceSettings.PhotoUri}/photo/{photoUrl}";
        }
    }
}
