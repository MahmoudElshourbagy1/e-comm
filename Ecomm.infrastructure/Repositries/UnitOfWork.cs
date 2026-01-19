using AutoMapper;
using Ecom.Core.Entites;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecomm.infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using StackExchange.Redis;

namespace Ecomm.infrastructure.Repositries
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IImageManagementService _imageManagementService;
        private readonly IConnectionMultiplexer _redis;
        private readonly UserManager<AppUser> userManager;
        private readonly IEmailService emailService;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IGenerateToken token;



        public UnitOfWork(AppDbContext context, IMapper mapper, IImageManagementService imageManagementService, IConnectionMultiplexer redis, UserManager<AppUser> userManager, IEmailService emailService, SignInManager<AppUser> signInManager, IGenerateToken token = null)
        {
            _context = context;
            _mapper = mapper;
            _imageManagementService = imageManagementService;
            _redis = redis;
            this.userManager = userManager;
            this.emailService = emailService;
            this.signInManager = signInManager;
            this.token = token;
            productRepositry = new ProductRepositry(_context, _mapper, _imageManagementService);
            categoryRepositry = new CategoryRepositry(_context);
            photoRepositry = new PhotoRepositry(_context);
            customerBasket = new CustomerBasketRepository(redis);
            Auth = new AuthRepositry(userManager, emailService, signInManager, token,context);
          
        }
        public IProductRepositry productRepositry { get; }

        public ICategoryRepositry categoryRepositry { get; }

        public IPhotoRepositry photoRepositry { get; }

        public ICustomerBasketRepository customerBasket { get; }

        public IAuth Auth { get; }
    }
}
