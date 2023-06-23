using UserInterface.Models.Payment;

namespace UserInterface.MicroserviceCommunication.Abstract
{
    public interface IPaymentService
    {
        Task<bool> GetPayment(PaymentModel paymentModel);

    }
}
