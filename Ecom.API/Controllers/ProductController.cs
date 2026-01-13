using AutoMapper;
using Ecom.API.Helper;
using Ecom.Core.DTO;
using Ecom.Core.Entites.Product;
using Ecom.Core.Interfaces;
using Ecom.Core.Sharing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{

    public class ProductController : BaseController
    {
        public ProductController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }
        [HttpGet("get-all")]
        public async Task<IActionResult> get([FromQuery]ProductParams productParams)
        {
            try
            {
                var Products = await work.productRepositry.
                    GetAllAsync(productParams);
              
                return Ok(new Pagination<ProductDTO>(productParams.PageSize,productParams.PageNumber, Products.TotalCount, Products.products));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> getById(int id)
        {
            try
            {
                var product = await work.productRepositry.GetByIdAsync(id, x => x.Category, x => x.Photos);
                var result = mapper.Map<ProductDTO>(product);
                if (product is null)
                {
                    return BadRequest(new ResponseAPI(400));
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPost("Add-product")]
        public async Task<IActionResult> add(AddProductDTO productDTO)
        {
            try
            {
                await work.productRepositry.AddAsync(productDTO);
                return Ok(new ResponseAPI(200));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPut("Update-Product")]
        public async Task<IActionResult> Update(UpdateProductDTO updateProductDTO)
        {
            try
            {
                await work.productRepositry.UpdateAsync(updateProductDTO);
                return Ok(new ResponseAPI(200));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpDelete("Delete-Product/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var product = await work.productRepositry.GetByIdAsync(id,x=>x.Photos,x=>x.Category);
                await work.productRepositry.DeleteAsync(product);

                return Ok(new ResponseAPI(200));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
