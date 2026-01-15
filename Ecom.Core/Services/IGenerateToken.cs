using Ecom.Core.Entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Core.Services
{
    public interface IGenerateToken
    {
        string GetAndCreateToken(AppUser user);
    }
}
