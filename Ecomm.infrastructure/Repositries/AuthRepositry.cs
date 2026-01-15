using Castle.Core.Smtp;
using Ecom.Core.DTO;
using Ecom.Core.Entites;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.Core.Sharing;
using MailKit;
using Microsoft.AspNetCore.Identity;
using System.Web;

namespace Ecomm.infrastructure.Repositries
{
    public class AuthRepositry : IAuth
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IEmailService emailService;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IGenerateToken generateToken;

        public AuthRepositry(UserManager<AppUser> userManager, IEmailService emailService, SignInManager<AppUser> signInManager, IGenerateToken generateToken = null)
        {
            this.userManager = userManager;
            this.emailService = emailService;
            this.signInManager = signInManager;
            this.generateToken = generateToken;
        }
        public async Task<string> RegisterAsync(RegisterDTO registerDTO)
        {
            if (registerDTO == null) {
                return null;
            }
            if (await userManager.FindByNameAsync(registerDTO.UserName) is not null) {
                return "this username is already registerd";
            }
            if (await userManager.FindByEmailAsync(registerDTO.Email) is not null)
            {
                return "this Email is already registerd";
            }
            AppUser user = new()
            {
                Email = registerDTO.Email,
                UserName = registerDTO.UserName,
                DisplayName= registerDTO.DisplayName,
            };
            var result = await userManager.CreateAsync(user, registerDTO.Password);
            if (result.Succeeded is not true)
            {
                return result.Errors.ToList()[0].Description;
            }
            // send Active Email
            string token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            await SendEmail(user.Email, token, "active", "ActiveEmail", "Plase active your email , click on button to active");

            return "done";
        }
        public async Task SendEmail(string email, string code, string component, string subject, string message) {
            var reslut = new EmailDTO(email
                , "mahmoudmera555@gmail.com",
                subject,
                EmailStringBody.send(email, code, component, message)
                );
            await emailService.sendEmail(reslut);
        }
        public async Task<string> LoginAsync(LoginDTo loginDTo)
        {
            if (loginDTo == null)
            {
                return null;
            }
            var finduser = await userManager.FindByEmailAsync(loginDTo.Email);
            if (!finduser.EmailConfirmed)
            {
                string token = await userManager.GenerateEmailConfirmationTokenAsync(finduser);
                await SendEmail(finduser.Email, token, "active", "ActiveEmail", "Plase active your email , click on button to active");
                return "please confirem your email frist , we have send activat to your email";

            }
            var reslut = await signInManager.CheckPasswordSignInAsync(finduser, loginDTo.Password, true);
            if (reslut.Succeeded)
            {
                return generateToken.GetAndCreateToken(finduser);
            }

            return "please check your email and passowrd , something went worng";
        }
        public async Task<bool> SendEmailForForgetPassowrd(string email)
        {
            var findUser = await userManager.FindByEmailAsync(email);
            if (findUser is null) {
                return false;
            }
            var token = await userManager.GeneratePasswordResetTokenAsync(findUser);
            await SendEmail(findUser.Email, token, "Reset-Password", "Reset Password", "click on button to Reset Password");
            return true;
        }
        public async Task<string> ResetPassword(ResetPasswordDTO resetPassword)
        {
            var findUser = await userManager.FindByEmailAsync(resetPassword.Email);
            if (findUser is null)
            {
                return null;
            }
            resetPassword.Token = HttpUtility.UrlDecode(resetPassword.Token);
            resetPassword.Token = resetPassword.Token.Replace(" ", "+");
            var result = await userManager.ResetPasswordAsync(findUser, resetPassword.Token, resetPassword.Password);
            if (result.Succeeded) {
                return "Password change success";
            }
            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                    Console.WriteLine("Password Error: " + err.Description);
            }
            return result.Errors.ToList()[0].Description;

        }
        public async Task<bool> ActiveAccount(ActiveAccountDTO accountDTO)
        {
            var findUser = await userManager.FindByEmailAsync(accountDTO.Email);
            if (findUser is null)
            {
                return false;
            }
            var result = await userManager.ConfirmEmailAsync(findUser, accountDTO.Token);
            if (result.Succeeded)
            {
                return true;
            }
            var token = await userManager.GenerateEmailConfirmationTokenAsync(findUser);
            await SendEmail(findUser.Email, token, "active", "ActiveEmail", "Plase active your email , click on button to active");
            return false;
        }
    }
}
