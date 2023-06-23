namespace CarRental.Services.Catalog.Dtos
{
    public class CarUpdatedDto
    {
        public string? Id { get; set; }
        public string? UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public decimal Price { get; set; }
        public FeatureDto? Feature { get; set; }
        public DateTime? RentDate { get; set; }
        public DateTime? LeaveDate { get; set; }
        public string Location { get; set; }
        public string CategoryId { get; set; }
        public string? Picture { get; set; }
        public IFormFile? PhotoFormFile { get; set; }
        public decimal? TotalPricePaid { get; set; }
        public bool IsPassive { get; set; } = false;
    }
}
