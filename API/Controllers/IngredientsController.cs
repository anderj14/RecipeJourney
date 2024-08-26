
using API.Dtos;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class IngredientsController : BaseApiController
    {
        private readonly IGenericRepository<Ingredient> _ingredientRepo;
        private readonly IMapper _mapper;

        public IngredientsController(IGenericRepository<Ingredient> ingredientRepo, IMapper mapper)
        {
            _ingredientRepo = ingredientRepo;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IngredientDto>> GetIngredient(int id)
        {
            var ingredient = await _ingredientRepo.GetByIdAsync(id);

            if (ingredient == null) return NotFound(new ApiResponse(404, "Ingredient Not Found"));

            var data = _mapper.Map<IngredientDto>(ingredient);

            return Ok(data);
        }

        [HttpPost]
        public async Task<ActionResult<IngredientDto>> CreateIngredient([FromBody] CreateIngredientDto createIngredientDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid data"));

            var newIngredient = _mapper.Map<CreateIngredientDto, Ingredient>(createIngredientDto);
            _ingredientRepo.Create(newIngredient);
            await _ingredientRepo.SaveAsync();

            var data = _mapper.Map<IngredientDto>(newIngredient);

            return CreatedAtAction(nameof(GetIngredient), new { id = newIngredient.Id }, data);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<IngredientDto>> UpdateIngredient(int id, CreateIngredientDto updateIngredientDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid data"));
            try
            {
                var spec = new IngredientSpecification(id);
                var existingIngredient = await _ingredientRepo.GetEntityWithSpecAsync(spec);

                if (existingIngredient == null) return NotFound(new ApiResponse(404, "Ingredient Not Found"));

                _mapper.Map(updateIngredientDto, existingIngredient);

                _ingredientRepo.Update(existingIngredient);
                await _ingredientRepo.SaveAsync();

                var data = _mapper.Map<IngredientDto>(existingIngredient);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> UpdateIngredient(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid data"));
            try
            {
                var spec = new IngredientSpecification(id);
                var existingIngredient = await _ingredientRepo.GetEntityWithSpecAsync(spec);

                if (existingIngredient == null) return NotFound(new ApiResponse(404, "Ingredient Not Found"));

                _ingredientRepo.Delete(existingIngredient);
                await _ingredientRepo.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}