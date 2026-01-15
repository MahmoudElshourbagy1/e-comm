using Ecom.Core.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Core.Services
{
    public interface IEmailService
    {
        Task sendEmail(EmailDTO emailDTO);
    }
}
