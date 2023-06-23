using UserInterface.MicroserviceCommunication.Abstract;
using UserInterface.Models.Payment;

namespace UserInterface.MicroserviceCommunication.Concrete
{
    public class PaymentService : IPaymentService
    {
        private readonly HttpClient _httpClient;

        public PaymentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> GetPayment(PaymentModel paymentModel)
        {
            var response = await _httpClient.PostAsJsonAsync<PaymentModel>("payments", paymentModel);
            return response.IsSuccessStatusCode;
        }
    }
}
