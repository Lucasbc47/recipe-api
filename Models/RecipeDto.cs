using System.ComponentModel.DataAnnotations;

namespace Recipe.API.Models
{
    public class RecipeDto
    {
        public string Id { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MinLength(1)]
        public List<string> Ingredients { get; set; } = new();

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        [MinLength(1)]
        public List<string> Steps { get; set; } = new();

        public List<string> Images { get; set; } = new();

        public List<string> Videos { get; set; } = new();
    }

    public class CreateRecipeDto
    {
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MinLength(1)]
        public List<string> Ingredients { get; set; } = new();

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        [MinLength(1)]
        public List<string> Steps { get; set; } = new();

        public List<string> Images { get; set; } = new();

        public List<string> Videos { get; set; } = new();
    }

    public class UpdateRecipeDto
    {
        [StringLength(100, MinimumLength = 1)]
        public string? Name { get; set; }

        public List<string>? Ingredients { get; set; }

        [StringLength(300)]
        public string? Description { get; set; }

        public List<string>? Steps { get; set; }

        public List<string>? Images { get; set; }

        public List<string>? Videos { get; set; }
    }
}