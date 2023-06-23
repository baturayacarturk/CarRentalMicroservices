namespace UserInterface.Models.CatalogModels
{
    public class CarViewModel
    {
        public string? Id { get; set; }
        public string? UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public decimal Price { get; set; }
        public string Picture { get; set; }
        
        public string PictureUrl { get; set; }
        public DateTime? CreatedTime { get; set; }
        public FeatureViewModel? Feature { get; set; }
        public string? Location { get; set; }
        public string CategoryId { get; set; }

        public CategoryViewModel? Category { get; set; }
        public DateTime? RentDate { get; set; }
        public decimal? TotalPricePaid { get; set; }
        public DateTime? LeaveDate { get; set; }
        public bool? IsPassive { get; set; }
    }
}
