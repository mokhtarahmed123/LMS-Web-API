using LMS.API.Bases;
using LMS.Core.Feature.Categories.Command.Models;
using LMS.Core.Feature.Categories.Query;
using LMS.Core.Feature.Categories.Query.Models;
using LMS.Core.Feature.Categories.Query.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Swashbuckle.AspNetCore.Annotations;

namespace LMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : AppBaseController
    {
        private string? UserId => CurrentUserService.UserIdFromJWTWithNull();
        private string cacheKey => $"all_categories_user_{UserId}";
        private string FilterCacheKey => $"all_categories_filter_user_{UserId}";
        private string PaginatedCacheKey => $"all_categories_paginated_user_{UserId}";



        #region Post
        [Authorize(Roles = "Admin")]
        [EnableRateLimiting("FixedWindowPolicy")]
        [SwaggerOperation(
          Summary = "Creates a new Category",
          Description = "This endpoint allows you to create a new category and store it in the database."
      )]
        [SwaggerResponse(201, "Category Added Successfully")]
        [SwaggerResponse(400, "Invalid data provided")]
        [SwaggerResponse(500, "An unexpected error occurred")]
        [HttpPost]

        public async Task<IActionResult> AddCategory([FromBody] AddCategoryCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        #endregion
        #region Get
        [HttpGet]
        [EnableRateLimiting("SlidingWindowPolicy")]
        [SwaggerOperation(Summary = "Get all categories", Description = "Retrieve all categories from the database.")]
        [SwaggerResponse(200, "List of categories returned successfully", type: typeof(List<GetAllCategoriesQueryResult>))]
        public async Task<IActionResult> GetAllCategories()
        {

            var Category = await RedisCacheService.GetDataAsync<List<GetAllCategoriesQueryResult>>(cacheKey);
            if (Category != null)
            {
                return Ok(Category);
            }

            var response = await Mediator.Send(new GetAllCategoriesQuery());

            if (response != null)
            {
                await RedisCacheService.SetDataAsync(cacheKey, response.Data, TimeSpan.FromMinutes(10));
            }
            return NewResult(response);
        }

        [SwaggerOperation(Summary = "Get category by ID", Description = "Retrieve a specific category by its ID.")]
        [SwaggerResponse(200, "Category found successfully", typeof(GetCategoryByIdQuery))]
        [SwaggerResponse(400, "ID must be greater than 0")]
        [SwaggerResponse(404, "Category not found")]
        [EnableRateLimiting("SlidingWindowPolicy")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] int id)
        {
            var response = await Mediator.Send(new GetCategoryByIdQuery(id));


            return NewResult(response);
        }



        [HttpGet("Filter")]
        [EnableRateLimiting("TokenBucketPolicy")]
        public async Task<IActionResult> GetAllCategoriesByFilter([FromQuery] GetAllCategoriesByFilter Query)
        {
            var Category = await RedisCacheService.GetDataAsync<List<AllCategoriesByFilterResponse>>(FilterCacheKey);
            if (Category != null)
            {
                return Ok(Category);
            }
            var response = await Mediator.Send(Query);
            if (response != null)
            {
                await RedisCacheService.SetDataAsync(cacheKey, response.Data, TimeSpan.FromMinutes(10));
            }
            return NewResult(response);

        }
        [HttpGet("Paginated")]
        [EnableRateLimiting("TokenBucketPolicy")]
        public async Task<IActionResult> GetAllCategoriesPaginated([FromQuery] GetAllCategoriesPaginatedQuery Query)
        {
            var Category = await RedisCacheService.GetDataAsync<List<GetAllCategoriesPaginatedResult>>(PaginatedCacheKey);
            if (Category != null)
            {
                return Ok(Category);
            }

            var response = await Mediator.Send(Query);
            if (response != null)
            {
                await RedisCacheService.SetDataAsync(cacheKey, response.Data, TimeSpan.FromMinutes(10));
            }

            return Ok(response);

        }


        #endregion

        #region Put
        [Authorize(Roles = "Admin")]
        [EnableRateLimiting("FixedWindowPolicy")]
        [SwaggerOperation(Summary = "Update a category", Description = "Update an existing category using its ID.")]
        [SwaggerResponse(200, "Category updated successfully")]
        [SwaggerResponse(400, "Invalid data or ID")]
        [SwaggerResponse(404, "Category not found")]
        [SwaggerResponse(500, "Unexpected server error")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory([FromRoute] int id, [FromBody] UpdateCategoryCommand updateCategory)
        {

            updateCategory.Id = id;
            var response = await Mediator.Send(updateCategory);
            if (response != null) ClearCache(); ;

            return NewResult(response);
        }
        #endregion

        #region Delete
        [Authorize(Roles = "Admin")]
        [EnableRateLimiting("FixedWindowPolicy")]
        [SwaggerOperation(Summary = "Delete a category", Description = "Delete a specific category using its ID.")]
        [SwaggerResponse(200, "Category deleted successfully")]
        [SwaggerResponse(400, "ID must be greater than 0")]
        [SwaggerResponse(404, "Category not found")]
        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteCategory([FromRoute] int id)
        {
            var response = await Mediator.Send(new DeleteCategoryCommand(id));
            if (response != null) await ClearCache();

            return NewResult(response);
        }
        #endregion

        private async Task ClearCache()
        {

            await RedisCacheService.RemoveAsync(cacheKey);
            await RedisCacheService.RemoveAsync(FilterCacheKey);
            await RedisCacheService.RemoveAsync(PaginatedCacheKey);
        }


    }
}