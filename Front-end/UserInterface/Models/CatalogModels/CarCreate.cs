using System.ComponentModel.DataAnnotations;

namespace UserInterface.Models.CatalogModels
{
    public class CarCreate
    {
        public string? UserId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
   
        public decimal Price { get; set; }
        public FeatureViewModel? Feature { get; set; }
    
        public string CategoryId { get; set; }

        public string Location { get; set; }
        public string? Picture { get; set; }
        public IFormFile? PhotoFormFile { get; set; }
        public bool IsPassive { get; set; }
    }
}
