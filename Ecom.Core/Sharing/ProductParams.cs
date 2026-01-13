using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Core.Sharing
{
    public class ProductParams
    {
        //[FromQuery] string? sort,int? CategoryId,int PageSize,int PageNumber
        public string? Sort { get; set; }
        public int? CategoryId { get; set; }
        public string? Search {  get; set; }
        public int MaxPageSize { get; set; } = 6;
        private int _pageSize=3;
        public int PageSize { get { return _pageSize; } set { _pageSize = value>MaxPageSize?MaxPageSize:value; } }
        public int PageNumber { get; set; } = 1;
    }
}
