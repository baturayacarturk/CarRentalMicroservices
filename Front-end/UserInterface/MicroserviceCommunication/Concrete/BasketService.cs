using CarRental.Shared.Dtos;
using CarRental.Shared.Services;
using System.Net.Http;
using UserInterface.MicroserviceCommunication.Abstract;
using UserInterface.Models.Baskets;

namespace UserInterface.MicroserviceCommunication.Concrete
{
    public class BasketService : IBasketService
    {
        private readonly HttpClient _httpClient;
        private readonly IDiscountService _discountService;
        private readonly ISharedIdentityService _sharedIdentityService;


        public BasketService(HttpClient httpClient, ISharedIdentityService sharedIdentityService, IDiscountService discountService)
        {
            _httpClient = httpClient;
            _sharedIdentityService = sharedIdentityService;
            _discountService = discountService;
        }

        public async Task AddBasketItem(BasketItemViewModel basketItemViewModel)
        {
            var basket = await GetBasket();

            if (basket != null)
            {
                if (!basket.BasketItesm.Any(x => x.CarId == basketItemViewModel.CarId))
                {
                    basket.BasketItesm.Add(basketItemViewModel);
                }
            }
            else
            {
                basket = new BasketViewModel();

                basket.BasketItesm.Add(basketItemViewModel);
            }

            await SaveOrUpdateBasket(basket);
        }


        public async Task<bool> ApplyDiscount(string code)
        {
            await CancelTheDiscount();
            var basket = await GetBasket();
            if (basket == null )
            {
                return false;
            }
            var hasDiscount = await _discountService.GetDiscount(code);
            if (hasDiscount == null)
            {
                return false;
            }
            basket.ApplyDiscount(hasDiscount.Code, hasDiscount.Rate);
            await SaveOrUpdateBasket(basket);
            return true;
        }

        public async Task<bool> CancelTheDiscount()
        {
            var basket = await GetBasket();
            if (basket == null || basket.DiscountCode == null)
            {
                return false;
            }
            basket.CancelDiscount();

            await SaveOrUpdateBasket(basket);
            return true;

        }

        public async Task<bool> DeleteBasket(BasketViewModel basketViewModel)
        {
            var result = await _httpClient.DeleteAsync("baskets");
            return result.IsSuccessStatusCode;
        }

        public async Task<BasketViewModel> GetBasket()
        {
            var response = await _httpClient.GetAsync("baskets");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var basketViewModel = await response.Content.ReadFromJsonAsync<ResponseDto<BasketViewModel>>();
            return basketViewModel.Data;

        }

        public async Task<bool> RemoveBasketItem(string carId)
        {
            var basket = await GetBasket();
            if (basket == null)
            {
                return false;
            }
            var delete = basket.BasketItesm.Remove(basket.BasketItesm.FirstOrDefault(x => x.CarId == carId));
            if (!delete || delete == null)
            {
                return false;
            }
            if (!basket.BasketItesm.Any())
            {
                basket.DiscountCode = null;
            }
            await SaveOrUpdateBasket(basket);
            return true;
        }

        public async Task<bool> SaveOrUpdateBasket(BasketViewModel basketViewModel)
        {
            basketViewModel.UserId = _sharedIdentityService.GetUserId;
            var response = await _httpClient.PostAsJsonAsync<BasketViewModel>("baskets", basketViewModel);
            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
            }
            return response.IsSuccessStatusCode;
        }

    }
}
