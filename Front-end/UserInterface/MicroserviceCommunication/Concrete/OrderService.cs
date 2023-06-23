using CarRental.Shared.Dtos;
using CarRental.Shared.Services;
using NuGet.Protocol;
using UserInterface.MicroserviceCommunication.Abstract;
using UserInterface.Models.CatalogModels;
using UserInterface.Models.Orders;
using UserInterface.Models.Payment;

namespace UserInterface.MicroserviceCommunication.Concrete
{
    public class OrderService : IOrderService
    {
        private readonly IPaymentService _paymentService;
        private readonly HttpClient _httpClient;
        private readonly IBasketService _basketService;
        private readonly ISharedIdentityService _sharedIdentityService;
        private readonly ICatalogService _catalogService;
        public OrderService(IPaymentService paymentService, HttpClient httpClient, IBasketService basketService, ISharedIdentityService sharedIdentityService, ICatalogService catalogService)

        {
            _catalogService = catalogService;
            _sharedIdentityService = sharedIdentityService;
            _paymentService = paymentService;
            _httpClient = httpClient;
            _basketService = basketService;
        }

        public async Task<OrderCreatedViewModel> CreateOrder(CheckOutModel checkoutInformation)
        {
            var basket = await _basketService.GetBasket();
            var payment = new PaymentModel
            {
                CardName = checkoutInformation.CardName,
                CardNumber = checkoutInformation.CardNumber,
                Expiration = checkoutInformation.Expiration,
                CVV = checkoutInformation.CVV,
                TotalPrice = basket.TotalPrice
            };
            var response = await _paymentService.GetPayment(payment);
            if (!response)
            {
                return new OrderCreatedViewModel { Error = "Ödeme alınırken hata oluştu.", IsSuccessful = false };
            }

            var orderCreate = new OrderCreateInput()
            {
                RenterId = _sharedIdentityService.GetUserId,
                Address= new AddressCreateInput

                {
                    Provience = checkoutInformation.Province,
                    District = checkoutInformation.District,
                    Street = checkoutInformation.Street,
                    Line = checkoutInformation.Line,
                    ZipCode = checkoutInformation.ZipCode,

                }

            };
            var newCarList = new List<string>();
            foreach (var item in basket.BasketItesm)
            {
                var car = await _catalogService.GetByCarId(item.CarId);
                newCarList.Add(car.Id);
            }


            basket.BasketItesm.ForEach(x =>
            {


                var orderItem = new OrderItemCreateInput
                {
                    ProductId = x.CarId,
                    Price = x.CurrentPrice,
                    PictureUrl = "",
                    ProductName = x.CarName,
                    LeaveDate = x.LeaveDate,
                };
                orderCreate.OrderItems.Add(orderItem);
            });
            var responseMessage = await _httpClient.PostAsJsonAsync<OrderCreateInput>("orders", orderCreate);
            if (!responseMessage.IsSuccessStatusCode)
            {
                return new OrderCreatedViewModel { Error = "Bir problem meydana geldi", IsSuccessful = false };
            }


            foreach (var basketCar in basket.BasketItesm)
            {
                foreach (var carId in newCarList)
                {
                    if (basketCar.CarId == carId)
                    {
                        var currentCar = await _catalogService.GetByCarId(carId);
                        var updateCar = new CarUpdate
                        {
                            Id = currentCar.Id,
                            UserId = currentCar.UserId,
                            Name = currentCar.Name,
                            Description = currentCar.Description,
                            Location = currentCar.Location,
                            CategoryId = currentCar.CategoryId,
                            Price = currentCar.Price,
                            Picture = currentCar.Picture,
                            RentDate = basketCar.RentDate,
                            LeaveDate = basketCar.LeaveDate,
                            IsPassive = false,
                            TotalPricePaid = basketCar.Price
                        };
                        await _catalogService.UpdateCarAsync(updateCar);
                    }

                }

            }
            var orderCreated = await responseMessage.Content.ReadFromJsonAsync<OrderCreatedViewModel>();
            orderCreated.IsSuccessful = true;
            await _basketService.DeleteBasket(basket);
            return orderCreated;
        }

        public async Task<List<OrderModel>> GetOrder()
        {
            var response = await _httpClient.GetFromJsonAsync<ResponseDto<List<OrderModel>>>("orders");
            return response.Data;
        }

        public async Task<OrderQueueModel> QueueOrder(CheckOutModel checkoutInformation)
        {

            var basket = await _basketService.GetBasket();
            var orderCreate = new OrderCreateInput()
            {
                RenterId = _sharedIdentityService.GetUserId,
                Address = new AddressCreateInput

                {
                    Provience = checkoutInformation.Province,
                    District = checkoutInformation.District,
                    Street = checkoutInformation.Street,
                    Line = checkoutInformation.Line,
                    ZipCode = checkoutInformation.ZipCode,

                }

            };
            basket.BasketItesm.ForEach(x =>
            {


                var orderItem = new OrderItemCreateInput
                {
                    ProductId = x.CarId,
                    Price = x.CurrentPrice,
                    PictureUrl = "",
                    ProductName = x.CarName,
                    LeaveDate = x.LeaveDate,
                };
                orderCreate.OrderItems.Add(orderItem);
            });

            var payment = new PaymentModel
            {
                CardName = checkoutInformation.CardName,
                CardNumber = checkoutInformation.CardNumber,
                Expiration = checkoutInformation.Expiration,
                CVV = checkoutInformation.CVV,
                Order = orderCreate,
                TotalPrice = basket.TotalPrice
            };
            var response = await _paymentService.GetPayment(payment);
            if (!response)
            {
                return new OrderQueueModel { Error = "Ödeme alınırken hata oluştu.", IsSuccessful = false };
            }
            var newCarList = new List<string>();
            foreach (var item in basket.BasketItesm)
            {
                var car = await _catalogService.GetByCarId(item.CarId);
                newCarList.Add(car.Id);
            }
            foreach (var basketCar in basket.BasketItesm)
            {
                foreach (var carId in newCarList)
                {
                    if (basketCar.CarId == carId)
                    {
                        var currentCar = await _catalogService.GetByCarId(carId);
                        var updateCar = new CarUpdate
                        {
                            Id = currentCar.Id,
                            UserId = currentCar.UserId,
                            Name = currentCar.Name,
                            Description = currentCar.Description,
                            Location = currentCar.Location,
                            CategoryId = currentCar.CategoryId,
                            Price = currentCar.Price,
                            Picture = currentCar.Picture,
                            RentDate = basketCar.RentDate,
                            LeaveDate = basketCar.LeaveDate,
                            IsPassive = false,
                            TotalPricePaid = basketCar.Price
                        };
                        await _catalogService.UpdateCarAsync(updateCar);
                    }

                }

            }
            await _basketService.DeleteBasket(basket);
            return new OrderQueueModel { IsSuccessful = true };
        }
    }
}
