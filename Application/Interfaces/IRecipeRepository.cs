using RecipeModel = Recipe.API.Models.Recipe;

namespace Recipe.API.Application.Interfaces
{
    public interface IRecipeRepository
    {
        Task<List<RecipeModel>> GetAllAsync();
        Task<RecipeModel?> GetByIdAsync(string id);
        Task CreateAsync(RecipeModel recipe);
        Task UpdateAsync(string id, RecipeModel recipe);
        Task DeleteAsync(string id);
    }

}
