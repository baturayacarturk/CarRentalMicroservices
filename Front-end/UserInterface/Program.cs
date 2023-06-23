using CarRental.Shared.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using UserInterface.Extensions;
using UserInterface.Handlers;
using UserInterface.Helpers;
using UserInterface.MicroserviceCommunication.Abstract;
using UserInterface.MicroserviceCommunication.Concrete;
using UserInterface.Models;
using UserInterface.Validations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<MicroServiceApiAdjustment>(builder.Configuration.GetSection("MicroServiceApiAdjustment"));
builder.Services.Configure<UserSettings>(builder.Configuration.GetSection("UserSettings"));
builder.Services.AddHttpContextAccessor();
builder.Services.AddAccessTokenManagement();

builder.Services.AddScoped<ISharedIdentityService, SharedIdentityService>();
builder.Services.AddScoped<IPhotoService, PhotoService>();

builder.Services.AddControllersWithViews().AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<CarCreateValidator>());

builder.Services.AddScoped<CredentialHandler>();

builder.Services.AddScoped<PasswordTokenHandler>();
builder.Services.AddSingleton<PhotoHelpers>();

builder.Services.AddHttpClient<IIdentityServiceComm, IdentityServiceComm>();
builder.Services.AddHttpClient<ICredentialService, CredentialService>();

builder.Services.AddServices(builder.Configuration);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, op => { op.LoginPath = "/Authentication/SigningIn"; op.ExpireTimeSpan = TimeSpan.FromDays(50); op.SlidingExpiration = true; op.Cookie.Name = "carrentalcookie"; });



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    
}
app.UseExceptionHandler("/Home/Error");
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=LocationList}/{id?}");

app.Run();
