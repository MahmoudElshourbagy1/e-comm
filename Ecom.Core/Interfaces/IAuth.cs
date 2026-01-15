using Ecom.Core.DTO;
using Ecom.Core.Entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Core.Interfaces
{
    public interface IAuth
    {
        Task<string> RegisterAsync(RegisterDTO registerDTO);
        Task<string> LoginAsync(LoginDTo loginDTo);
        Task<bool> SendEmailForForgetPassowrd(string email);
        Task<string> ResetPassword(ResetPasswordDTO resetPassword);
        Task<bool> ActiveAccount(ActiveAccountDTO accountDTO);
        Task SendEmail(string email, string code, string component, string subject, string message);

    }
}
