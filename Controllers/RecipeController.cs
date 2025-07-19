using Microsoft.AspNetCore.Mvc;
using Recipe.API.Application.Services;
using Recipe.API.Models;

using RecipeModel = Recipe.API.Models.Recipe;

namespace Recipe.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly RecipeService _recipeService;
        private readonly ILogger<RecipeController> _logger;

        public RecipeController(RecipeService recipeService, ILogger<RecipeController> logger)
        {
            _recipeService = recipeService;
            _logger = logger;
        }

        /// <summary>
        /// Get all recipes
        /// </summary>
        /// <returns>A ResponseBase containing a list of all recipes on success, or an error message on failure</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseBase<List<RecipeDto>>>> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all recipes");
                var recipes = await _recipeService.GetAllAsync();
                var recipeDtos = recipes.Select(MapToDto).ToList();
                return Ok(ResponseBase<List<RecipeDto>>.CreateSuccess(recipeDtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all recipes");
                return StatusCode(500, ResponseBase<List<RecipeDto>>.CreateError("An error occurred while retrieving recipes"));
            }
        }

        /// <summary>
        /// Get a recipe by ID
        /// </summary>
        /// <param name="id">Recipe ID</param>
        /// <returns>A ResponseBase containing the recipe on success, or an error message on failure</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseBase<RecipeDto>>> GetById(string id)
        {
            try
            {
                _logger.LogInformation("Getting recipe with ID: {Id}", id);
                var recipe = await _recipeService.GetByIdAsync(id);
                
                if (recipe == null)
                {
                    return NotFound(ResponseBase<RecipeDto>.CreateError($"Recipe with ID {id} not found"));
                }
                
                return Ok(ResponseBase<RecipeDto>.CreateSuccess(MapToDto(recipe)));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid recipe ID: {Id}", id);
                return BadRequest(ResponseBase<RecipeDto>.CreateError(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting recipe with ID: {Id}", id);
                return StatusCode(500, ResponseBase<RecipeDto>.CreateError("An error occurred while retrieving the recipe"));
            }
        }

        /// <summary>
        /// Create a new recipe
        /// </summary>
        /// <param name="createDto">Recipe data to create</param>
        /// <returns>A ResponseBase containing the created recipe on success, or an error message on failure</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseBase<RecipeDto>>> Create([FromBody] CreateRecipeDto createDto)
        {
            try
            {
                _logger.LogInformation("Creating new recipe: {Name}", createDto.Name);
                var recipe = MapToModel(createDto);
                var createdRecipe = await _recipeService.CreateAsync(recipe);
                return CreatedAtAction(
                    nameof(GetById), 
                    new { id = createdRecipe.Id }, 
                    ResponseBase<RecipeDto>.CreateSuccess(MapToDto(createdRecipe)));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid recipe data");
                return BadRequest(ResponseBase<RecipeDto>.CreateError(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating recipe");
                return StatusCode(500, ResponseBase<RecipeDto>.CreateError("An error occurred while creating the recipe"));
            }
        }

        /// <summary>
        /// Update an existing recipe
        /// </summary>
        /// <param name="id">Recipe ID</param>
        /// <param name="updateDto">Updated recipe data</param>
        /// <returns>A ResponseBase containing the updated recipe on success, or an error message on failure</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseBase<RecipeDto>>> Update(string id, [FromBody] UpdateRecipeDto updateDto)
        {
            try
            {
                _logger.LogInformation("Updating recipe with ID: {Id}", id);
                var recipe = MapToModel(updateDto);
                recipe.Id = id; // Ensure the ID is set for the update
                var updatedRecipe = await _recipeService.UpdateAsync(id, recipe);
                
                if (updatedRecipe == null)
                {
                    return NotFound(ResponseBase<RecipeDto>.CreateError($"Recipe with ID {id} not found"));
                }
                
                return Ok(ResponseBase<RecipeDto>.CreateSuccess(MapToDto(updatedRecipe)));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid data for recipe update");
                return BadRequest(ResponseBase<RecipeDto>.CreateError(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating recipe with ID: {Id}", id);
                return StatusCode(500, ResponseBase<RecipeDto>.CreateError("An error occurred while updating the recipe"));
            }
        }

        /// <summary>
        /// Delete a recipe
        /// </summary>
        /// <param name="id">Recipe ID</param>
        /// <returns>A ResponseBase indicating success, or an error message on failure</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseBase>> Delete(string id)
        {
            try
            {
                _logger.LogInformation("Deleting recipe with ID: {Id}", id);
                await _recipeService.DeleteAsync(id);
                return Ok(ResponseBase.CreateSuccess());
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid recipe ID for deletion: {Id}", id);
                return BadRequest(ResponseBase.CreateError(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting recipe with ID: {Id}", id);
                return StatusCode(500, ResponseBase.CreateError("An error occurred while deleting the recipe"));
            }
        }

        private static RecipeDto MapToDto(RecipeModel recipe)
        {
            return new RecipeDto
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Ingredients = recipe.Ingredients,
                Description = recipe.Description,
                Steps = recipe.Steps,
                Images = recipe.Images,
                Videos = recipe.Videos
            };
        }

        private static RecipeModel MapToModel(CreateRecipeDto dto)
        {
            return new RecipeModel
            {
                Name = dto.Name,
                Ingredients = dto.Ingredients,
                Description = dto.Description,
                Steps = dto.Steps,
                Images = dto.Images,
                Videos = dto.Videos
            };
        }

        private static RecipeModel MapToModel(UpdateRecipeDto dto)
        {
            return new RecipeModel
            {
                Name = dto.Name,
                Ingredients = dto.Ingredients,
                Description = dto.Description,
                Steps = dto.Steps,
                Images = dto.Images,
                Videos = dto.Videos
            };
        }
    }
}