using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Values;

var builder = WebApplication.CreateBuilder(args);


builder.Configuration.AddJsonFile($"Configuration.{builder.Environment.EnvironmentName}.json").AddEnvironmentVariables();

// Ocelot configuration
builder.Services.AddOcelot();
builder.Services.AddAuthentication().AddJwtBearer("GatewayAuthenticationScheme", options =>
{
    options.Authority = builder.Configuration["IdentityServerURL"];
    options.Audience = "gatweway_source";
    options.RequireHttpsMetadata = false;
});
var app = builder.Build();


// Ocelot middleware
app.UseOcelot();
app.Run();
