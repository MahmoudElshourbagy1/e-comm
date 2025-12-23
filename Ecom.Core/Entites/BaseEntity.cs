using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Core.Entites
{
    public class BaseEntity<T>
    {
        public T Id {  get; set; }
    }
}
