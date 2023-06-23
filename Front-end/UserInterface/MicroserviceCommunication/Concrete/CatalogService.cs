using CarRental.Shared.Dtos;
using CarRental.Shared.Services;
using System.Net.Http.Json;
using UserInterface.Helpers;
using UserInterface.MicroserviceCommunication.Abstract;
using UserInterface.Models;
using UserInterface.Models.CatalogModels;

namespace UserInterface.MicroserviceCommunication.Concrete
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _httpClient;
        private readonly IPhotoService _photoService;
        private readonly PhotoHelpers photoHelpers;
        
        public CatalogService(HttpClient httpClient, IPhotoService photoService,PhotoHelpers photo)
        {
            _httpClient = httpClient;
            _photoService = photoService;
            photoHelpers = photo;
  
        }

        public async Task<bool> AddCarAsync(CarCreate carCreate)
        {
            var photoResult = await _photoService.UploadImage(carCreate.PhotoFormFile);
            if(photoResult != null)
            {
                carCreate.Picture = photoResult.Url;
            }

            var response = await _httpClient.PostAsJsonAsync<CarCreate>("car/create", carCreate);
            return response.IsSuccessStatusCode;
        }
        public async Task<List<CarViewModel>> GetAllCarsBasedOnLocation(string location)
        {
            var response = await _httpClient.PostAsJsonAsync<string>("car/locationBased",location);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var successResponse = await response.Content.ReadFromJsonAsync<ResponseDto<List<CarViewModel>>>();
            successResponse.Data.ForEach(x =>
            {
                x.PictureUrl = photoHelpers.PhotoUrl(x.Picture);
            });
            return successResponse.Data;
        }
        public async Task<List<CarViewModel>> GetCurrentlyRentedCars(string userId)
        {
            var response = await _httpClient.PostAsJsonAsync<string>("car/currentRentedCars", userId);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var successResponse = await response.Content.ReadFromJsonAsync<ResponseDto<List<CarViewModel>>>();
            successResponse.Data.ForEach(x =>
            {
                x.PictureUrl = photoHelpers.PhotoUrl(x.Picture);
            });
            return successResponse.Data;
        }
        public async Task<List<CarViewModel>> GetAllPassiveCarsBasedOnUserId(string userId)
        {
            var response = await _httpClient.PostAsJsonAsync<string>("car/getPassiveCarListBasedOnUser", userId);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var successResponse = await response.Content.ReadFromJsonAsync<ResponseDto<List<CarViewModel>>>();
            successResponse.Data.ForEach(x =>
            {
                x.PictureUrl = photoHelpers.PhotoUrl(x.Picture);
            });
            return successResponse.Data;
        }
        public async Task<bool> DeleteCarAsync(string carId)
        {
            var response = await _httpClient.DeleteAsync($"car/{ carId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<CarViewModel>> GetAllCars()
        {
            var response = await _httpClient.GetAsync("car");
            

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            
            var successResponse = await response.Content.ReadFromJsonAsync<ResponseDto<List<CarViewModel>>>();
            successResponse.Data.ForEach(x =>
            {
                x.PictureUrl = photoHelpers.PhotoUrl(x.Picture);
            });
            return successResponse.Data;
        }

        public async Task<List<CarViewModel>> GetAllCarsByUserId(string UserId)
        {
           
            var response = await _httpClient.GetAsync($"car/GetAllByUserId/{UserId}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var successResponse = await response.Content.ReadFromJsonAsync<ResponseDto<List<CarViewModel>>>();
            successResponse.Data.ForEach(x =>
            {
                x.PictureUrl = photoHelpers.PhotoUrl(x.Picture);
            });
            return successResponse.Data; 
        }

        public async Task<List<CategoryViewModel>> GetAllCategoryAsync()
        {
            var response = await _httpClient.GetAsync("categories");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var successResponse = await response.Content.ReadFromJsonAsync<ResponseDto<List<CategoryViewModel>>>();

            return successResponse.Data;
        }

        public async Task<CarViewModel> GetByCarId(string carId)
        {
            var response = await _httpClient.GetAsync($"car/{carId}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var successResponse = await response.Content.ReadFromJsonAsync<ResponseDto<CarViewModel>>();

            successResponse.Data.PictureUrl = photoHelpers.PhotoUrl(successResponse.Data.Picture);
            return successResponse.Data;
        }

        public async Task<bool> UpdateCarAsync(CarUpdate carUpdate)
        {
            var photoResult = await _photoService.UploadImage(carUpdate.PhotoFormFile);
            if (photoResult != null)
            {
                await _photoService.DeleteImage(carUpdate.Picture);
                carUpdate.Picture = photoResult.Url;
            }
            var response = await _httpClient.PutAsJsonAsync<CarUpdate>("car", carUpdate);
            return response.IsSuccessStatusCode;
        }
    }
}
