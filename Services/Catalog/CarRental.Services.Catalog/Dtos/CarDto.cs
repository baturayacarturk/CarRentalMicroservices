
using MongoDB.Bson.Serialization.Attributes;

namespace CarRental.Services.Catalog.Dtos
{
    public class CarDto
    {

        public string? Id { get; set; }
        public string UserId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public decimal Price { get; set; }
        public string? Picture { get; set; }

        
        public DateTime? CreatedTime { get; set; }
        public DateTime? LeaveDate { get; set; }
        public DateTime? RentDate { get; set; }
        public string Location { get; set; }

        public FeatureDto? Feature { get; set; }

        public string CategoryId { get; set; }
        public decimal? TotalPricePaid { get; set; }
        public CategoryDto? Category { get; set; }
        public bool? IsPassive{ get; set; }
    }
}
