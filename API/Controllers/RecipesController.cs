
using API.Dtos;
using API.Dtos.CreateDtos;
using API.Errors;
using API.Extensions;
using API.Helper;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class RecipesController : BaseApiController
    {
        private readonly IGenericRepository<Recipe> _recipeRepo;
        private readonly IMapper _mapper;

        public RecipesController(IGenericRepository<Recipe> recipeRepo, IMapper mapper)
        {
            _recipeRepo = recipeRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<RecipeDto>>> GetRecipes([FromQuery] RecipeSpecParams recipeSpecParams)
        {
            var spec = new RecipeSpecification(recipeSpecParams);
            var countSpec = new RecipeFilterForCountSpecification(recipeSpecParams);

            var totalItems = await _recipeRepo.CountAsync(countSpec);

            if (totalItems == 0)
            {
                return Ok(new PagedList<RecipeDto>(new List<RecipeDto>(), 0, recipeSpecParams.PageIndex, recipeSpecParams.PageSize));
            }

            var recipes = await _recipeRepo.ListWithSpecAsync(spec);

            var data = _mapper.Map<IReadOnlyList<RecipeDto>>(recipes);

            var paginatedRecipe = new PagedList<RecipeDto>(
                data.ToList(),
                totalItems,
                recipeSpecParams.PageIndex,
                recipeSpecParams.PageSize
            );

            Response.AddPaginationHeader(paginatedRecipe.Metadata);

            return Ok(paginatedRecipe);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RecipeDto>> GetRecipe(int id)
        {
            var spec = new RecipeSpecification(id);
            var recipe = await _recipeRepo.GetEntityWithSpecAsync(spec);

            if (recipe == null) return NotFound(new ApiResponse(404, "Invalid Model"));

            var data = _mapper.Map<RecipeDto>(recipe);

            return Ok(data);
        }

        [HttpPost]
        public async Task<ActionResult> CreateRecipe([FromBody] CreateRecipeDto createRecipeDto)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiResponse(400, "Invalid Model"));
            try
            {
                var createRecipe = _mapper.Map<Recipe>(createRecipeDto);

                _recipeRepo.Create(createRecipe);
                createRecipe.CreatedDate = DateTime.Now;
                await _recipeRepo.SaveAsync();

                return CreatedAtAction(nameof(GetRecipe), new { id = createRecipe.Id }, createRecipe);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<RecipeDto>> UpdateRecipe(int id, CreateRecipeDto updateRecipeDto)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiResponse(404, "Invalid Model"));
            try
            {
                var spec = new RecipeSpecification(id);
                var existingRecipe = await _recipeRepo.GetEntityWithSpecAsync(spec);

                if (existingRecipe == null) return NotFound(new ApiResponse(404, "Recipe Not Found"));

                _mapper.Map(updateRecipeDto, existingRecipe);

                _recipeRepo.Update(existingRecipe);
                existingRecipe.CreatedDate = DateTime.Now;

                await _recipeRepo.SaveAsync();

                var data = _mapper.Map<RecipeDto>(existingRecipe);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRecipe(int id)
        {

            var spec = new RecipeSpecification(id);
            var existingRecipe = await _recipeRepo.GetEntityWithSpecAsync(spec);

            if (existingRecipe == null) return NotFound(new ApiResponse(404, "Recipe Not Found"));

            _recipeRepo.Delete(existingRecipe);
            await _recipeRepo.SaveAsync();

            return NoContent();
        }
    }
}