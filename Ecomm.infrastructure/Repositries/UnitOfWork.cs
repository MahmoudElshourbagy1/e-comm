using Ecom.Core.Interfaces;
using Ecomm.infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecomm.infrastructure.Repositries
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            productRepositry= new ProductRepositry(_context);
            categoryRepositry= new CategoryRepositry(_context);
            photoRepositry= new PhotoRepositry(_context);
        }
        public IProductRepositry productRepositry { get; }

        public ICategoryRepositry categoryRepositry { get; }

        public IPhotoRepositry photoRepositry { get; }

    }
}
