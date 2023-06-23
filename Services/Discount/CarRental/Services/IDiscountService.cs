using CarRental.Shared.Dtos;

namespace CarRental.Services.Discount.Services
{
    public interface IDiscountService
    {
        Task<ResponseDto<List<Model.Discount>>> GetAll();
        Task<ResponseDto<Model.Discount>> GetById(int id);
        Task<ResponseDto<NoContent>> Save(Model.Discount discount);
        Task<ResponseDto<NoContent>> Update(Model.Discount discount);
        Task<ResponseDto<NoContent>> Delete(int id);
        Task<ResponseDto<Model.Discount>> GetByCodeAndUserId(string code, string userId);
    }
}
