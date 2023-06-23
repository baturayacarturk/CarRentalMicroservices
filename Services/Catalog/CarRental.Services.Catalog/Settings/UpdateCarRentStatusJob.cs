using CarRental.Services.Catalog.Dtos;
using MongoDB.Driver;

namespace CarRental.Services.Catalog.Settings
{
    public class UpdateCarRentStatusJob
    {
        private readonly IMongoCollection<CarRental.Services.Catalog.Models.Car> _carCollection;
        private readonly IDatabaseSettings _databaseSettings;
        public UpdateCarRentStatusJob(IMongoDatabase database,IDatabaseSettings databaseSettings)
        {
            _databaseSettings = databaseSettings;
            _carCollection = database.GetCollection<CarRental.Services.Catalog.Models.Car>(databaseSettings.CarCollectionName);
        }

        public void Execute()
        {
            var filter = Builders<CarRental.Services.Catalog.Models.Car>.Filter.Eq(x => x.IsPassive, true);
            var update = Builders<CarRental.Services.Catalog.Models.Car>.Update.Inc(x => x.Feature.Duration, -1);

            var carsToUpdate = _carCollection.Find(filter).ToList();

            foreach (var car in carsToUpdate)
            {
                _carCollection.UpdateOne(x => x.Id == car.Id, update);

                if (car.Feature.Duration == 0)
                {
                    var rentedFilter = Builders<CarRental.Services.Catalog.Models.Car>.Filter.Eq(x => x.Id, car.Id) & Builders<CarRental.Services.Catalog.Models.Car>.Filter.Eq(x => x.IsPassive, true);
                    var rentedUpdate = Builders<CarRental.Services.Catalog.Models.Car>.Update.Set(x => x.IsPassive, false);

                    _carCollection.UpdateOne(rentedFilter, rentedUpdate);
                }
            }
        }
    }

}
