using UserInterface.Models.Photos;

namespace UserInterface.MicroserviceCommunication.Abstract
{
    public interface IPhotoService
    {
        Task<PhotoViewModel> UploadImage(IFormFile image);
        Task<bool> DeleteImage(string imageUrl);

    }
}
