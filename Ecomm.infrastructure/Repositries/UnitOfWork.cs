using AutoMapper;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecomm.infrastructure.Data;
using Ecomm.infrastructure.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecomm.infrastructure.Repositries
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IImageManagementService _imageManagementService;
        private readonly IConnectionMultiplexer _redis;

        public UnitOfWork(AppDbContext context, IMapper mapper, IImageManagementService imageManagementService,IConnectionMultiplexer redis)
        {
            _context = context;
            _mapper = mapper;
            _imageManagementService = imageManagementService;
            _redis = redis;
            productRepositry = new ProductRepositry(_context, _mapper, _imageManagementService);
            categoryRepositry = new CategoryRepositry(_context);
            photoRepositry = new PhotoRepositry(_context);
            customerBasket = new CustomerBasketRepository(redis);

        }
        public IProductRepositry productRepositry { get; }

        public ICategoryRepositry categoryRepositry { get; }

        public IPhotoRepositry photoRepositry { get; }

        public ICustomerBasketRepository customerBasket { get; }
    }
}
