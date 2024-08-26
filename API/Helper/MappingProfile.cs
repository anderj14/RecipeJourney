
using API.Dtos;
using API.Dtos.CreateDtos;
using AutoMapper;
using Core.Entities;

namespace API.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Recipe, RecipeDto>()
            .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category.CategoryName))
            .ForMember(d => d.Ingredients, o => o.MapFrom(s => s.Ingredients))
            .ForMember(d => d.Instructions, o => o.MapFrom(s => s.Instructions))
            .ForMember(d => d.Comments, o => o.MapFrom(s => s.Comments));
            CreateMap<Ingredient, IngredientDto>();
            CreateMap<Instruction, InstructionDto>();
            CreateMap<Comment, CommentDto>();

            CreateMap<CreateCategoryDto, Category>();
            CreateMap<CreateRecipeDto, Recipe>();
        }
    }
}