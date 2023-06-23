using UserInterface.Handlers;
using UserInterface.MicroserviceCommunication.Abstract;
using UserInterface.MicroserviceCommunication.Concrete;
using UserInterface.Models;

namespace UserInterface.Extensions
{
    public static class ServiceExtension
    {
        public static void AddServices(this IServiceCollection service, IConfiguration Configuration )
        {
            
            service.AddHttpClient<ICredentialService, CredentialService>();
            var apiSettings = Configuration.GetSection("MicroServiceApiAdjustment").Get<MicroServiceApiAdjustment>();

            service.AddHttpClient<IdentityServiceComm, IdentityServiceComm>(opts =>
            {

                opts.BaseAddress = new Uri(apiSettings.IdentityUri);
            }).AddHttpMessageHandler<PasswordTokenHandler>();

            service.AddHttpClient<IUserService, UserService>(opts =>
            {

                opts.BaseAddress = new Uri(apiSettings.IdentityUri);
            }).AddHttpMessageHandler<PasswordTokenHandler>();


            service.AddHttpClient<ICatalogService, CatalogService>(opts =>
            {
                opts.BaseAddress = new Uri($"{apiSettings.BaseUriGateway}/{apiSettings.Catalog.Path}");
            }).AddHttpMessageHandler<CredentialHandler>();

            service.AddHttpClient<IPhotoService, PhotoService>(opts =>
            {
                opts.BaseAddress = new Uri($"{apiSettings.BaseUriGateway}/{apiSettings.Photo.Path}");
            }).AddHttpMessageHandler<CredentialHandler>();
           
            service.AddHttpClient<IBasketService, BasketService>(opts =>
            {
               
                opts.BaseAddress = new Uri($"{apiSettings.BaseUriGateway}/{apiSettings.Basket.Path}");
            }).AddHttpMessageHandler<PasswordTokenHandler>();

            service.AddHttpClient<IOrderService, OrderService>(opts =>
            {
                opts.BaseAddress = new Uri($"{apiSettings.BaseUriGateway}/{apiSettings.Order.Path}");
            }).AddHttpMessageHandler<PasswordTokenHandler>();

            service.AddHttpClient<IDiscountService, DiscountService>(opts =>
            {
                opts.BaseAddress = new Uri($"{apiSettings.BaseUriGateway}/{apiSettings.Discount.Path}");
            }).AddHttpMessageHandler<PasswordTokenHandler>();

            service.AddHttpClient<IPaymentService, PaymentService>(opts =>
            {
                opts.BaseAddress = new Uri($"{apiSettings.BaseUriGateway}/{apiSettings.Payment.Path}");
            }).AddHttpMessageHandler<PasswordTokenHandler>();

        }
    }
}
