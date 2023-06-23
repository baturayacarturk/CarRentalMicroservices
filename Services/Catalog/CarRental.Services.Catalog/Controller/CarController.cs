
using CarRental.Services.Catalog.Dtos;
using CarRental.Services.Catalog.Services;
using CarRental.Shared.CustomController;
using CarRental.Shared.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Services.Car.Controller
{


    [Route("api/[controller]")]
    [ApiController]
    public class CarController : CustomBaseController
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var response = await _carService.GetByIdAsync(id);
            return CreateActionResultInstance(response);

        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _carService.GetAllAsync();


            return CreateActionResultInstance(response);

        }
        [HttpPost("currentRentedCars")]
        public async Task<IActionResult> GetRentCarList([FromBody] string userId)
        {
            var response = await _carService.GetRentCarList(userId);
            return CreateActionResultInstance(response);
        }
        [HttpPost("locationBased")]
        public async Task<IActionResult> GetAllCarsBasedOnLocation([FromBody]string location)
        {
            var response = await _carService.GetAllCarsBasedOnLocation(location);
            return CreateActionResultInstance(response);
        }

        [HttpPost("getPassiveCarListBasedOnUser")]
        public async Task<IActionResult> GetPassiveCarList([FromBody]string userId)

        {
            var response = await _carService.GetPassiveCarsByUser(userId);
            return CreateActionResultInstance(response);
        }

        [HttpGet]
        [Route("/api/[controller]/GetAllByUserId/{userId}")]
        public async Task<IActionResult> GetAllByUserId(string userId)
        {
            var response = await _carService.GetAllByUserId(userId);
            return CreateActionResultInstance(response);

        }
        [HttpPost("create")]
        public async Task<IActionResult> Create(CarDto carCreatedDto)
        {
            var response = await _carService.CreateAsync(carCreatedDto);
            return CreateActionResultInstance(response);

        }

        [HttpPut]
        public async Task<IActionResult> Update(CarUpdatedDto updateCreateDto)
        {
            if (updateCreateDto.RentDate == null)
            {
                updateCreateDto.RentDate=DateTime.MinValue;
                updateCreateDto.LeaveDate = DateTime.MinValue;
            }

            var response = await _carService.UpdateAsync(updateCreateDto);
            return CreateActionResultInstance(response);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _carService.DeleteByIdAsync(id);
            return CreateActionResultInstance(response);

        }

    }
}
