using Ecom.Core.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Core.Interfaces
{
    public interface IRating
    {
        Task<bool> AddRatingAsync(RatingDTO ratingDTO, string email);
        Task<IReadOnlyList<ReturnRatingDTO>> GetAllRatingForProductAsync(int productId);
    }
}
