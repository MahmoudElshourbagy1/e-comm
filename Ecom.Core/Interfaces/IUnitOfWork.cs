using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Core.Interfaces
{
    public interface IUnitOfWork
    {
        public IProductRepositry productRepositry { get; }
        public ICategoryRepositry categoryRepositry { get; }
        public IPhotoRepositry photoRepositry { get; }

    }
}
