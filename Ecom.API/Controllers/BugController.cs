using AutoMapper;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BugController : BaseController
    {
        public BugController(IUnitOfWork work, IMapper mapper)
            : base(work, mapper)
        {
        }

        // 404 - Not Found
        [HttpGet("not-found")]
        public async Task<IActionResult> GetNotFound()
        {
            var category = await work.categoryRepositry.GetByIdAsync(100);

            if (category == null)
                return NotFound();

            return Ok(category);
        }

        // 500 - Server Error (مقصود للتجربة)
        [HttpGet("server-error")]
        public async Task<IActionResult> ServerError()
        {
            var category = await work.categoryRepositry.GetByIdAsync(100);

            if (category == null)
                throw new Exception("Category not found");

            category.Name = null; 
            return Ok(category);
        }

        // 400 - Bad Request
        [HttpGet("bad-request")]
        public IActionResult GetBadRequest()
        {
            return BadRequest();
        }

        // 400 - Bad Request with id
        [HttpGet("bad-request/{id}")]
        public IActionResult GetBadRequestById(int id)
        {
            return BadRequest($"Invalid id: {id}");
        }
    }
}
