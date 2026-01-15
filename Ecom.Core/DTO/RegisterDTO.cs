using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Core.DTO
{
    public record LoginDTo
    {
        public string Password { get; set; }
        public string Email { get; set; }
    }
    public record RegisterDTO : LoginDTo
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }

    }
    public record ResetPasswordDTO: LoginDTo
    {
        public string Token { get; set; }
    }
    public record ActiveAccountDTO
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
