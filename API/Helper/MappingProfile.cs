
using API.Dtos;
using API.Dtos.CreateDtos;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;

namespace API.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Recipe, RecipeDto>()
            .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category.CategoryName));
            CreateMap<Ingredient, IngredientDto>();
            CreateMap<Instruction, InstructionDto>();
            CreateMap<Comment, CommentDto>()
            .ForMember(d => d.UserName, o => o.MapFrom(s => s.AppUser));
            CreateMap<AppUser, UserDto>();
            CreateMap<UserFavoriteRecipe, UserFavoriteRecipeDto>();

            CreateMap<CreateCategoryDto, Category>();
            CreateMap<CreateRecipeDto, Recipe>();
            CreateMap<CreateIngredientDto, Ingredient>();
            CreateMap<CreateInstructionDto, Instruction>();
            CreateMap<CreateCommentDto, Comment>();
            CreateMap<CreateUserFavoriteRecipeDto, UserFavoriteRecipe>();
        }
    }
}