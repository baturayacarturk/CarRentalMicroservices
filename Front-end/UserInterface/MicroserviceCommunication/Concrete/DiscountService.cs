using CarRental.Shared.Dtos;
using UserInterface.MicroserviceCommunication.Abstract;
using UserInterface.Models.Discount;

namespace UserInterface.MicroserviceCommunication.Concrete
{
    public class DiscountService : IDiscountService
    {
        private readonly HttpClient _httpClient;

        public DiscountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<DiscountViewModel> GetDiscount(string discountCode)
        {
            var response = await _httpClient.GetAsync($"discounts/GetByCode/{discountCode}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var discount = await response.Content.ReadFromJsonAsync<ResponseDto<DiscountViewModel>>();
            return discount.Data;
        }
    }
}
