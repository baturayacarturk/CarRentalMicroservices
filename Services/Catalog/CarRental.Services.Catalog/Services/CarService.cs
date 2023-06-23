using AutoMapper;
using CarRental.Services.Catalog.Dtos;
using CarRental.Services.Catalog.Models;
using CarRental.Services.Catalog.Settings;
using CarRental.Shared.Dtos;
using CarRental.Shared.Services;
using MongoDB.Driver;

namespace CarRental.Services.Catalog.Services
{
    public class CarService : ICarService
    {
        private readonly IMongoCollection<CarRental.Services.Catalog.Models.Car> _car;
        private readonly IMapper _mapper;
        private readonly IMongoCollection<Category> _category;
        private readonly ISharedIdentityService _sharedIdentityService;
        public CarService(IMapper mapper, IDatabaseSettings databaseSettings, ISharedIdentityService sharedIdentityService)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _category = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);


            _car = database.GetCollection<CarRental.Services.Catalog.Models.Car>(databaseSettings.CarCollectionName);
            _mapper = mapper;
            _sharedIdentityService = sharedIdentityService;
        }

        public async Task<ResponseDto<List<CarDto>>> GetAllAsync()
        {

            var listOfCars = await _car.Find(car => true).ToListAsync();
            listOfCars = listOfCars.FindAll(x => x.IsPassive == true&& x.RentDate==DateTime.MinValue).ToList();

            if (listOfCars.Any())
            {
                foreach (var item in listOfCars)
                {
                    item.Category = await _category.Find<Category>(x => x.Id == item.CategoryId).FirstAsync();

                }
            }
            return ResponseDto<List<CarDto>>.Success(_mapper.Map<List<CarDto>>(listOfCars), 200);
        }

        public async Task<ResponseDto<List<CarDto>>> GetPassiveCarsByUser(string userId)
        {
            var listOfCars = await _car.Find(x => x.UserId == userId && x.IsPassive==false&& x.RentDate==DateTime.MinValue).ToListAsync();

            if (listOfCars.Any())
            {
                foreach (var item in listOfCars)
                {
                    item.Category = await _category.Find<Category>(x => x.Id == item.CategoryId).FirstAsync();

                }
            }
            return ResponseDto<List<CarDto>>.Success(_mapper.Map<List<CarDto>>(listOfCars), 200);


        }
        public async Task<ResponseDto<List<CarDto>>> GetRentCarList(string userId)
        {
            var listOfCars = await _car.Find(x => x.UserId == userId && x.RentDate!=DateTime.MinValue).ToListAsync();

            if (listOfCars.Any())
            {
                foreach (var item in listOfCars)
                {
                    item.Category = await _category.Find<Category>(x => x.Id == item.CategoryId).FirstAsync();

                }
            }
            return ResponseDto<List<CarDto>>.Success(_mapper.Map<List<CarDto>>(listOfCars), 200);


        }
        public async Task<ResponseDto<List<CarDto>>>GetAllCarsBasedOnLocation(string location)
        {

            var listOfCars = await _car.Find(car => true).ToListAsync();
            listOfCars = listOfCars.FindAll(x => x.IsPassive == true).Where(x=>x.Location == location).ToList();//

            if (listOfCars.Any())
            {
                foreach (var item in listOfCars)
                {
                    item.Category = await _category.Find<Category>(x => x.Id == item.CategoryId).FirstAsync();

                }
            }
            return ResponseDto<List<CarDto>>.Success(_mapper.Map<List<CarDto>>(listOfCars), 200);
        }
        public async Task<ResponseDto<CarDto>> GetByIdAsync(string id)
        {
            var car = await _car.Find<CarRental.Services.Catalog.Models.Car>(x => x.Id == id).FirstOrDefaultAsync();
            if (car == null)
            {
                return ResponseDto<CarDto>.Fail("Araba bulunamadı", 404);

            }
            car.Category = await _category.Find<Category>(x => x.Id == car.CategoryId).FirstAsync();
            return ResponseDto<CarDto>.Success(_mapper.Map<CarDto>(car), 200);
        }
        public async Task<ResponseDto<List<CarDto>>> GetAllByUserId(string userId)
        {
            var car = await _car.Find<CarRental.Services.Catalog.Models.Car>(x => x.UserId == userId).ToListAsync();
            if (car.Any())
            {
                foreach (var item in car)
                {
                    item.Category = await _category.Find<Category>(x => x.Id == item.CategoryId).FirstAsync();

                }
            }
            return ResponseDto<List<CarDto>>.Success(_mapper.Map<List<CarDto>>(car), 200);

        }
        public async Task<ResponseDto<CarDto>> CreateAsync(CarDto carDto)
        {
            var newCar = _mapper.Map<CarRental.Services.Catalog.Models.Car>(carDto);
            newCar.CreatedTime = DateTime.Now;
            newCar.RentDate = DateTime.MinValue;
            newCar.LeaveDate= DateTime.MinValue;
            await _car.InsertOneAsync(newCar);
            return ResponseDto<CarDto>.Success(_mapper.Map<CarDto>(newCar), 200);
        }
        public async Task<ResponseDto<NoContent>> UpdateAsync(CarUpdatedDto carUpdateDto)
        {
            var updateCar = _mapper.Map<CarRental.Services.Catalog.Models.Car>(carUpdateDto);
            var result = await _car.FindOneAndReplaceAsync(x => x.Id == carUpdateDto.Id, updateCar);

            if (result == null)
            {
                return ResponseDto<NoContent>.Fail("Araba Bulunamadı", 404);
            }
            return ResponseDto<NoContent>.Success(204);
        }
        public async Task<ResponseDto<NoContent>> DeleteByIdAsync(string id)
        {
            var result = await _car.DeleteOneAsync(x => x.Id == id);
            if (result.DeletedCount > 0)
            {
                return ResponseDto<NoContent>.Success(204);
            }
            else
            {
                return ResponseDto<NoContent>.Fail("Araba Bulunamadı", 404);
            }
        }


    }
}
