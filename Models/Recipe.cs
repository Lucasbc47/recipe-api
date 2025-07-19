using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Recipe.API.Models;
public class Recipe
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public List<string> Ingredients { get; set; } = new();

    public string? Description { get; set; }

    public List<string> Steps { get; set; } = new();

    public List<string> Images { get; set; } = new();

    public List<string> Videos { get; set; } = new();
}
