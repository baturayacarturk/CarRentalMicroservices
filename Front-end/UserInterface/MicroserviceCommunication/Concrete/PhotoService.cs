using CarRental.Shared.Dtos;
using System.Security.Policy;
using UserInterface.MicroserviceCommunication.Abstract;
using UserInterface.Models.Photos;

namespace UserInterface.MicroserviceCommunication.Concrete
{
    public class PhotoService : IPhotoService
    {
        private readonly HttpClient _httpClient;

        public PhotoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> DeleteImage(string imageUrl)
        {
            var response = await _httpClient.DeleteAsync($"photos?photoUrl={imageUrl}");
            return response.IsSuccessStatusCode;
        }

        public async Task<PhotoViewModel> UploadImage(IFormFile photo)
        {
            if (photo == null || photo.Length <= 0)
            {
                return null;
            }
            var createFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(photo.FileName)}";
            using (var stream = new MemoryStream())
            {
                await photo.CopyToAsync(stream);
                var partContent = new MultipartFormDataContent();
                partContent.Add(new ByteArrayContent(stream.ToArray()), "photo", createFileName);//
                var response = await _httpClient.PostAsync("photos/save", partContent);
                if (!response.IsSuccessStatusCode)
                {
                    var errors = await response.Content.ReadAsStringAsync();
                    return null;
                }
                else
                {
                    var errors = await response.Content.ReadAsStringAsync();
                }
                var success = await response.Content.ReadFromJsonAsync<ResponseDto<PhotoViewModel>>();
                return success.Data;
            }
        }
    }
}
