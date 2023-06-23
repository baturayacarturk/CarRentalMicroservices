using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CarRental.Services.Catalog.Models
{
    public class Car
    {
        
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonId]
        public string Id { get; set; }
        public string UserId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        [BsonRepresentation(MongoDB.Bson.BsonType.Decimal128)]
        public decimal Price { get; set; }
        public string Picture { get; set; }
        
        public DateTime? RentDate { get; set; }
        public DateTime? LeaveDate { get; set; }
        

        [BsonRepresentation(MongoDB.Bson.BsonType.DateTime)]
        public DateTime CreatedTime { get; set; }
        public string Location { get; set; }
        public Feature? Feature { get; set; }
        

        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string CategoryId { get; set; }
        public bool? IsPassive { get; set; }
        public decimal? TotalPricePaid { get; set; }
        [BsonIgnore]
        public Category Category { get; set; }
    }
}
