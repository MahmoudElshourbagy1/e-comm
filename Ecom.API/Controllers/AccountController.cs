using AutoMapper;
using Ecom.API.Helper;
using Ecom.Core.DTO;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult>login(LoginDTo loginDTo)
        {
            var result = await work.Auth.LoginAsync(loginDTo);
            if(result.StartsWith("please"))
            {
                return BadRequest(new ResponseAPI(400,result));
            }
            Response.Cookies.Append("Token", result, new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                Domain="localhost",
                Expires=DateTime.Now.AddDays(1),
                IsEssential = true,
                SameSite=SameSiteMode.Strict,

            });
            return Ok(new ResponseAPI(200, result));

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
    }
}
