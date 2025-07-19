using MongoDB.Driver;
using Microsoft.Extensions.Options;
using Recipe.API.Models;

using RecipeModel = Recipe.API.Models.Recipe;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        _database = client.GetDatabase(settings.Value.DatabaseName);
    }
    public IMongoCollection<RecipeModel> Recipes =>_database.GetCollection<RecipeModel>("Recipes");
}
