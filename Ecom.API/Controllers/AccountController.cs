using AutoMapper;
using Ecom.API.Helper;
using Ecom.Core.DTO;
using Ecom.Core.Entites;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecom.API.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }
        [HttpPost("Register")]
        public async Task<IActionResult> register(RegisterDTO registerDTO)
        {
            string result = await work.Auth.RegisterAsync(registerDTO);
            if(result != "done")
            {
                return BadRequest(new ResponseAPI(400, result));

            }
            return Ok(new ResponseAPI(200, result));
        }
        [HttpPost("Login")]
        public async Task<IActionResult> login(LoginDTo loginDTo)
        {
            var (Succeeded, Message) = await work.Auth.LoginAsync(loginDTo);

            if (!Succeeded)
                return BadRequest(new ResponseAPI(400, Message));

            
            Response.Cookies.Append("Token", Message, new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                Domain = "localhost",
                Expires = DateTime.Now.AddDays(1),
                IsEssential = true,
                SameSite = SameSiteMode.Strict,
            });

            return Ok(new ResponseAPI(200, Message));
        }
        [HttpPost("active-account")]
        public async Task<IActionResult> active(ActiveAccountDTO accountDTO)
        {
            var result = await work.Auth.ActiveAccount(accountDTO);
            return result ? Ok(new ResponseAPI(200)) : BadRequest(new ResponseAPI(400));

        }
        [HttpGet("send-email-forget-password")]
        public async Task<IActionResult> forget(string email)
        {
            var result = await work.Auth.SendEmailForForgetPassowrd(email);
            return result ? Ok(new ResponseAPI(200)) : BadRequest(new ResponseAPI(400));

        }
       
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            var result = await work.Auth.ResetPassword(resetPasswordDTO);

            if (result == "Password change success")
            {
                return Ok(new ResponseAPI(200, result));
            }

            return BadRequest(new ResponseAPI(400, result));
        }
        [HttpPut("update-address")]
               public async Task<IActionResult> UpdateAddress(ShipAddressDTO addressDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                return Unauthorized(new ResponseAPI(401, "Email not found in token"));

            var address = mapper.Map<Address>(addressDTO);
            var result = await work.Auth.UpdateAddress(email, address);

            if (!result)
                return BadRequest(new ResponseAPI(400, "Failed to update address"));

            return Ok(new ResponseAPI(200, "Address updated successfully"));
        }
        [HttpGet("whoami")]
        public IActionResult WhoAmI()
        {
            return Ok(User.Claims.Select(c => new { c.Type, c.Value }));
        }
        [HttpGet("get-address-for-user")]
        public async Task<IActionResult> GetAddress()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(email))
                return Unauthorized("Email claim not found");

            var address = await work.Auth.getUserAddress(email);

            if (address == null)
                return NotFound("No address found for this user");

            var result = mapper.Map<ShipAddressDTO>(address);

            return Ok(result);
        }
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("Token", new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                Domain = "localhost",
                SameSite = SameSiteMode.Strict,
                IsEssential = true
            });

            return Ok(new ResponseAPI(200, "Logged out successfully"));
        }
        [Authorize]
        [HttpGet("get-user-name")]
        public IActionResult GetUserName()
        {
            return Ok(new ResponseAPI(200, User.Identity.Name));
        }
        [HttpGet("IsUserAuth")]
        public async Task<IActionResult> IsUserAuth()
        {

            return User.Identity.IsAuthenticated ? Ok() : BadRequest();
        }
    }
}
