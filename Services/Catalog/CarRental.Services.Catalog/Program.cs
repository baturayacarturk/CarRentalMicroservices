using CarRental.Services.Catalog.Services;
using CarRental.Services.Catalog.Settings;
using CarRental.Shared.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Options;
using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using MongoDB.Driver;

namespace CarRental.Services.Catalog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.Authority = builder.Configuration["IdentityServerURL"];
                options.Audience = "catalog_source";
                options.RequireHttpsMetadata = false;
            });

            builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<ISharedIdentityService, SharedIdentityService>();
            builder.Services.AddScoped<ICarService, CarService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            //builder.Services.AddScoped<UpdateCarRentStatusJob>();

            //builder.Services.AddSingleton<IMongoClient>(sp =>
            //{
            //    var databaseSettings = sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
            //    return new MongoClient(databaseSettings.ConnectionString);
            //});

            //builder.Services.AddScoped<IMongoDatabase>(sp =>
            //{
            //    var mongoClient = sp.GetRequiredService<IMongoClient>();
            //    var databaseSettings = sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
            //    return mongoClient.GetDatabase(databaseSettings.DatabaseName);
            //});
            builder.Services.AddSingleton<IDatabaseSettings>(sp =>
            {
                return sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
            });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAutoMapper(typeof(Program));

            //builder.Services.AddHangfire(config =>
            //{
            //    var options = new MongoStorageOptions
            //    {
            //        MigrationOptions = new MongoMigrationOptions
            //        {
            //            MigrationStrategy = new MigrateMongoMigrationStrategy(),
            //            BackupStrategy = new CollectionMongoBackupStrategy()
            //        }
            //    };

            //    var connectionString = builder.Configuration.GetSection("DatabaseSettings:ConnectionString").Value;
            //    var databaseName = builder.Configuration.GetSection("DatabaseSettings:DatabaseName").Value;


            //    config.UseMongoStorage(connectionString, databaseName, options);
            //});

            builder.Services.AddControllers(opt => opt.Filters.Add(new AuthorizeFilter()));

            var app = builder.Build();
            //using (var scope = app.Services.CreateScope())
            //{
            //var serviceProvider = scope.ServiceProvider;
            //var job = serviceProvider.GetRequiredService<UpdateCarRentStatusJob>();

            //app.UseHangfireServer();
            //app.UseHangfireDashboard();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());


            //RecurringJob.AddOrUpdate(() => job.Execute(), Cron.Daily);

            app.Run();
            //}
        }
    }
}
