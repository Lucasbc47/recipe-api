using RecipeModel = Recipe.API.Models.Recipe;
using Recipe.API.Application.Interfaces;
using MongoDB.Bson;

namespace Recipe.API.Application.Services
{
    public class RecipeService
    {
        private readonly IRecipeRepository _repo;
        
        public RecipeService(IRecipeRepository repo) 
        { 
            _repo = repo; 
        }
        
        public async Task<List<RecipeModel>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }
        
        public async Task<RecipeModel?> GetByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("Recipe ID cannot be null or empty", nameof(id));
                
            return await _repo.GetByIdAsync(id);
        }
        
        public async Task<RecipeModel> CreateAsync(RecipeModel recipe)
        {
            if (recipe == null)
                throw new ArgumentNullException(nameof(recipe));
                
            if (string.IsNullOrEmpty(recipe.Name))
                throw new ArgumentException("Recipe name is required", nameof(recipe));
            
            recipe.Id = ObjectId.GenerateNewId().ToString();                
            await _repo.CreateAsync(recipe);
            return recipe;
        }
        
        public async Task<RecipeModel?> UpdateAsync(string id, RecipeModel recipe)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("Recipe ID cannot be null or empty", nameof(id));
                
            if (recipe == null)
                throw new ArgumentNullException(nameof(recipe));
                
            await _repo.UpdateAsync(id, recipe);
            return await _repo.GetByIdAsync(id);
        }
        
        public async Task<bool> DeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("Recipe ID cannot be null or empty", nameof(id));
                
            await _repo.DeleteAsync(id);
            return true;
        }
    }
}
