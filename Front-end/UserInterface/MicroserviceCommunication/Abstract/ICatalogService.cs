using UserInterface.Models.CatalogModels;

namespace UserInterface.MicroserviceCommunication.Abstract
{
    public interface ICatalogService
    {
        Task<List<CarViewModel>> GetAllCars();
        Task<List<CarViewModel>>GetAllCarsByUserId(string userId);
        
        Task<CarViewModel> GetByCarId(string carId);
        Task<List<CategoryViewModel>> GetAllCategoryAsync();
        Task<bool> DeleteCarAsync(string carId);
        Task<bool> AddCarAsync(CarCreate carCreate);
        Task<bool> UpdateCarAsync(CarUpdate carUpdate);
        Task<List<CarViewModel>> GetAllCarsBasedOnLocation(string location);
        Task<List<CarViewModel>> GetAllPassiveCarsBasedOnUserId(string userId);
        Task<List<CarViewModel>> GetCurrentlyRentedCars(string userId);
    }
}
