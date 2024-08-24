
using API.Dtos.CreateDtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CategoriesController : BaseApiController
    {
        private readonly IGenericRepository<Category> _categoryRepo;
        private readonly IMapper _mapper;

        public CategoriesController(IGenericRepository<Category> categoryRepo, IMapper mapper)
        {
            _categoryRepo = categoryRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Category>>> GetCategories()
        {
            var categories = await _categoryRepo.ListAllAsync();

            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _categoryRepo.GetByIdAsync(id);

            if (category == null) return NotFound();

            return Ok(category);
        }


        [HttpPost]
        public async Task<ActionResult> CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var categoryCreate = _mapper.Map<Category>(createCategoryDto);

                _categoryRepo.Create(categoryCreate);
                _categoryRepo.SaveAsync();

                return CreatedAtAction(nameof(GetCategory), new { id = categoryCreate.Id }, categoryCreate);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCategory(int id, CreateCategoryDto updateCategoryDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var categorySearched = await _categoryRepo.GetByIdAsync(id);

                if (categorySearched == null) return NotFound("Category not found");

                _mapper.Map(updateCategoryDto, categorySearched);


                _categoryRepo.Update(categorySearched);
                _categoryRepo.SaveAsync();

                return Ok(categorySearched);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var categorySearched = await _categoryRepo.GetByIdAsync(id);

                if (categorySearched == null) return NotFound("Category not found");

                _categoryRepo.Delete(categorySearched);
                _categoryRepo.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}