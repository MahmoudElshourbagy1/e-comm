using Ecom.Core.Entites.Product;
using Ecom.Core.Interfaces;
using Ecomm.infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecomm.infrastructure.Repositries
{
    public class ProductRepositry : GenericRepositry<Product>, IProductRepositry
    {
        public ProductRepositry(AppDbContext context) : base(context)
        {
        }
    }
}
