using CarRental.Services.Catalog.Dtos;
using CarRental.Shared.Dtos;

namespace CarRental.Services.Catalog.Services
{
    public interface ICarService
    {
        Task<ResponseDto<List<CarDto>>> GetAllAsync();
        Task<ResponseDto<CarDto>> GetByIdAsync(string id);
        Task<ResponseDto<List<CarDto>>> GetAllByUserId(string userId);
        Task<ResponseDto<CarDto>> CreateAsync(CarDto carDto);
        Task<ResponseDto<NoContent>> UpdateAsync(CarUpdatedDto carUpdateDto);
        Task<ResponseDto<NoContent>> DeleteByIdAsync(string id);
        Task<ResponseDto<List<CarDto>>> GetAllCarsBasedOnLocation(string location);
        Task<ResponseDto<List<CarDto>>> GetPassiveCarsByUser(string userId);
        Task<ResponseDto<List<CarDto>>> GetRentCarList(string userId);
    }
}
