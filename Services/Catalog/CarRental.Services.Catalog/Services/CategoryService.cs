using AutoMapper;
using CarRental.Services.Catalog.Dtos;
using CarRental.Services.Catalog.Models;
using CarRental.Services.Catalog.Settings;
using CarRental.Shared.Dtos;
using MongoDB.Driver;

namespace CarRental.Services.Catalog.Services
{
    public class CategoryService:ICategoryService
    {
        private readonly IMongoCollection<Category> _category;
        private readonly IMapper _mapper;

        public CategoryService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _category = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
            _mapper = mapper;
        }

        public async Task<ResponseDto<List<CreatedCategoryDto>>> GetAllAsync()
        {
            var categories = await _category.Find(category => true).ToListAsync();
            return ResponseDto<List<CreatedCategoryDto>>.Success(_mapper.Map<List<CreatedCategoryDto>>(categories), 200);
        }
        public async Task<ResponseDto<CategoryDto>> CreateAsync(CategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            await _category.InsertOneAsync(category);
            return ResponseDto<CategoryDto>.Success(_mapper.Map<CategoryDto>(category), 201);
        }
        public async Task<ResponseDto<CategoryDto>> GetByIdAsync(string id)
        {
            var category = await _category.Find<Category>(x => x.Id == id).FirstOrDefaultAsync();
            return category == null ? ResponseDto<CategoryDto>.Fail("Category not found", 404) : ResponseDto<CategoryDto>.Success(_mapper.Map<CategoryDto>(category), 200);
        }
    }
}
