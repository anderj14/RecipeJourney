
using API.Dtos.CreateDtos;
using API.Errors;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _categoryRepo.GetByIdAsync(id);

            if (category == null) return NotFound(new ApiResponse(404, useSeriousMessages: false));

            return Ok(category);
        }


        [HttpPost]
        public async Task<ActionResult> CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiResponse(400, "Invalid model state"));

            try
            {
                var categoryCreate = _mapper.Map<Category>(createCategoryDto);

                await _categoryRepo.Create(categoryCreate);

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
            if (!ModelState.IsValid) return BadRequest(new ApiResponse(400, "Invalid model state"));

            try
            {
                var categorySearched = await _categoryRepo.GetByIdAsync(id);

                if (categorySearched == null) return NotFound(new ApiResponse(404, useSeriousMessages: false));

                _mapper.Map(updateCategoryDto, categorySearched);


                await _categoryRepo.Update(categorySearched);

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
            try
            {
                var categorySearched = await _categoryRepo.GetByIdAsync(id);

                if (categorySearched == null) return NotFound(new ApiResponse(404, useSeriousMessages: false));

                await _categoryRepo.Delete(categorySearched);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}