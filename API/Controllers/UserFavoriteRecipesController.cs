using API.Dtos;
using API.Dtos.CreateDtos;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class UserFavoriteRecipesController : BaseApiController
    {
        private readonly IGenericRepository<UserFavoriteRecipe> _userFavoriteRecipeRepo;
        private readonly IGenericRepository<Recipe> _recipeRepo;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public UserFavoriteRecipesController(IGenericRepository<UserFavoriteRecipe> userFavoriteRecipeRepo, IGenericRepository<Recipe> recipeRepo, UserManager<AppUser> userManager, IMapper mapper)
        {
            _userFavoriteRecipeRepo = userFavoriteRecipeRepo;
            _recipeRepo = recipeRepo;
            _userManager = userManager;
            _mapper = mapper;
        }

        protected async Task<AppUser> GetAuthenticatedUserAsync()
        {
            var email = User.GetEmail();

            if (string.IsNullOrEmpty(email))
            {
                return null;
            }

            return await _userManager.FindByEmailAsync(email);
        }

        [HttpPost]
        public async Task<ActionResult> CreateFavoriteRecipe([FromBody] CreateUserFavoriteRecipeDto createUserFavoriteRecipeDto)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiResponse(404, "Invalid Model"));
            try
            {
                var recipe = await _recipeRepo.GetByIdAsync(createUserFavoriteRecipeDto.RecipeId);

                var user = await GetAuthenticatedUserAsync();

                if (user == null)
                    return Unauthorized(new ApiResponse(401, "User not authenticated"));

                var existingFavorite = await _userFavoriteRecipeRepo.ListByConditionAsync(
                    ur => ur.RecipeId == createUserFavoriteRecipeDto.RecipeId && ur.UserId == user.Id
                );

                if (existingFavorite.Any())
                    return BadRequest(new ApiResponse(400, "Recipe already favorited"));

                var favoriteRecipe = new UserFavoriteRecipe
                {
                    RecipeId = createUserFavoriteRecipeDto.RecipeId,
                    UserId = user.Id,
                    CreatedDate = createUserFavoriteRecipeDto.CreatedDate
                };

                await _userFavoriteRecipeRepo.Create(favoriteRecipe);

                var data = _mapper.Map<UserFavoriteRecipeDto>(favoriteRecipe);

                return CreatedAtAction(nameof(GetFavoriteRecipesByUser), new { userId = user.Id }, data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("favorites")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserFavoriteRecipeDto>>> GetFavoriteRecipesByUser()
        {
            try
            {
                var user = await GetAuthenticatedUserAsync();

                if (user == null)
                {
                    return Unauthorized(new ApiResponse(401, useSeriousMessages: true));
                }

                var favoriteRecipes = await _userFavoriteRecipeRepo.ListByConditionAsync(ur => ur.UserId == user.Id);
                if (favoriteRecipes == null || !favoriteRecipes.Any())
                    return NotFound(new ApiResponse(404, useSeriousMessages: true));

                var data = _mapper.Map<IReadOnlyList<UserFavoriteRecipeDto>>(favoriteRecipes);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Internal server error: {ex.Message}"));
            }
        }

        [HttpDelete("recipeId")]
        public async Task<ActionResult> DeleteFavoriteRecipesByUser(int recipeId)
        {
            var user = await GetAuthenticatedUserAsync();
            if (user == null)
                return Unauthorized(new ApiResponse(401, useSeriousMessages: true));

            var favorite = await _userFavoriteRecipeRepo.ListByConditionAsync(ur => ur.RecipeId == recipeId && ur.UserId == user.Id);

            if (favorite == null || !favorite.Any())
                return NotFound(new ApiResponse(404, "Favorite recipe not found"));

            foreach (var fav in favorite)
            {
                await _userFavoriteRecipeRepo.Delete(fav);
            }

            return NoContent();
        }
    }
}