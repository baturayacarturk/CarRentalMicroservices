using CarRental.Services.Catalog.Dtos;
using CarRental.Services.Catalog.Services;
using CarRental.Shared.CustomController;

using Microsoft.AspNetCore.Mvc;

namespace CarRental.Services.Catalog.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : CustomBaseController
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();
            return CreateActionResultInstance(categories);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var categories = await _categoryService.GetByIdAsync(id);
            return CreateActionResultInstance(categories);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryDto categoryDto)
        {
            var categories = await _categoryService.CreateAsync(categoryDto);
            return CreateActionResultInstance(categories);
        }


    }
}
