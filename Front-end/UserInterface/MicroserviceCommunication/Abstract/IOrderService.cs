using UserInterface.Models.Orders;

namespace UserInterface.MicroserviceCommunication.Abstract
{
    public interface IOrderService
    {
        Task<OrderCreatedViewModel> CreateOrder(CheckOutModel checkoutInformation);
        Task<OrderQueueModel> QueueOrder(CheckOutModel checkoutInformation);//rabbitMQ
        Task<List<OrderModel>> GetOrder();
    }
}
