using AutoMapper;
using Castle.Components.DictionaryAdapter.Xml;
using Ecom.API.Helper;
using Ecom.Core.DTO;
using Ecom.Core.Entites.Product;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{
    public class CategoriesController : BaseController
    {
        public CategoriesController(IUnitOfWork work,IMapper mapper) : base(work ,mapper)
        {
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var categories = await work.categoryRepositry.GetAllAsync();
                if (categories is null || !categories.Any())
                {
                    return BadRequest(new ResponseAPI(400));
                }
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                var category = await work.categoryRepositry.GetByIdAsync(id);
                if (category is null)
                {
                    return BadRequest(new ResponseAPI(400, $"not found category id={id}"));
                }
                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPost("add-category")]
        public async Task<IActionResult> AddCategory(CategoryDto categoryDTO)
        {
            try
            {
                var category = mapper.Map<Category>(categoryDTO);
                await work.categoryRepositry.AddAsync(category);
                return Ok(new ResponseAPI (200,"Item has been added"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPut("update-category")]
        public async Task<IActionResult> UpdateCategory(int id, UpdateCategoryDto categoryDTO)
        {
            try
            {
              var category = mapper.Map<Category>(categoryDTO);
                await work.categoryRepositry.UpdateAsync(category);
                
                return Ok(new ResponseAPI(200, "Item has been updated"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpDelete("delete-category/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                await work.categoryRepositry.DeleteAsync(id);
                return Ok(new ResponseAPI(200, "Item has been deleted"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
