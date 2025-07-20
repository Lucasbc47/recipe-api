using RecipeModel = Recipe.API.Models.Recipe;
using Recipe.API.Application.Interfaces;
using MongoDB.Bson;
using Recipe.API.Models;

namespace Recipe.API.Application.Services;
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

        if (recipe.Ingredients == null || (recipe.Ingredients.Count == 0))
            throw new ArgumentException("Recipe ingredients are required", nameof(recipe));

        if (recipe.Steps == null || (recipe.Ingredients.Count == 0))
            throw new ArgumentException("Recipe steps are required", nameof(recipe));

        recipe.Id = ObjectId.GenerateNewId().ToString();
        await _repo.CreateAsync(recipe);
        return recipe;
    }

    public async Task<RecipeModel?> UpdateAsync(string id, UpdateRecipeDto dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        var existing = await _repo.GetByIdAsync(id);
        if (existing == null) return null;

        if (dto.Name != null) existing.Name = dto.Name;
        if (dto.Ingredients != null) existing.Ingredients = dto.Ingredients;
        if (dto.Description != null) existing.Description = dto.Description;
        if (dto.Steps != null) existing.Steps = dto.Steps;
        if (dto.Images != null) existing.Images = dto.Images;
        if (dto.Videos != null) existing.Videos = dto.Videos;

        await _repo.UpdateAsync(id, existing);
        return existing;
    }


    public async Task<bool> DeleteAsync(string id)
    {
        if (string.IsNullOrEmpty(id))
            throw new ArgumentException("Recipe ID cannot be null or empty", nameof(id));

        await _repo.DeleteAsync(id);
        return true;
    }
}