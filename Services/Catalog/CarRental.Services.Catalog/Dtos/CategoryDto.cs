using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace CarRental.Services.Catalog.Dtos
{
    public class CategoryDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
