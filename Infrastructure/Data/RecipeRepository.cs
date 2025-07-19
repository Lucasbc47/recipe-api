using MongoDB.Driver;
using Recipe.API.Application.Interfaces;
using Recipe.API.Models;
using Microsoft.Extensions.Options;

using RecipeModel = Recipe.API.Models.Recipe;

namespace Recipe.API.Infrastructure.Data
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly IMongoCollection<RecipeModel> _recipes;

        public RecipeRepository(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _recipes = database.GetCollection<RecipeModel>(settings.Value.CollectionName);
        }

        public async Task<List<RecipeModel>> GetAllAsync()
        {
            return await _recipes.Find(_ => true).ToListAsync();
        }

        public async Task<RecipeModel?> GetByIdAsync(string id)
        {
            return await _recipes.Find(r => r.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(RecipeModel recipe)
        {
            await _recipes.InsertOneAsync(recipe);
        }

        public async Task UpdateAsync(string id, RecipeModel recipe)
        {
            await _recipes.ReplaceOneAsync(r => r.Id == id, recipe);
        }

        public async Task DeleteAsync(string id)
        {
            await _recipes.DeleteOneAsync(r => r.Id == id);
        }
    }
}
