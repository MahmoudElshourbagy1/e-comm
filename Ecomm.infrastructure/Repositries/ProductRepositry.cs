using AutoMapper;
using Ecom.Core.DTO;
using Ecom.Core.Entites.Product;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.Core.Sharing;
using Ecomm.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecomm.infrastructure.Repositries
{
    public class ProductRepositry : GenericRepositry<Product>, IProductRepositry
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;
        private readonly IImageManagementService imageManagementService;
        public ProductRepositry(AppDbContext context, IMapper mapper, IImageManagementService imageManagementService)
     : base(context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.imageManagementService = imageManagementService ?? throw new ArgumentNullException(nameof(imageManagementService));
        }

        public async Task<bool> AddAsync(AddProductDTO productDTO)
        {
            if (productDTO == null) return false;
            var product = mapper.Map<Product>(productDTO);
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();
            var ImagePath = await imageManagementService.AddImageAsync(productDTO.Photos, productDTO.Name);
            var photo = ImagePath.Select(path => new Photo
            {
                ImageName = path,
                ProductId = product.Id
            }).ToList();
            await context.Photos.AddRangeAsync(photo);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task DeleteAsync(Product product)
        {
            var photos = await context.Photos.Where(p => p.ProductId == product.Id).ToListAsync();
            foreach (var photo in photos)
            {
                imageManagementService.DeleteImageAsync(photo.ImageName);
            }
            context.Products.Remove(product);
            await context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(UpdateProductDTO updateProductDTO)
        {
            if (updateProductDTO == null) return false;

            var findProduct = await context.Products
                .Include(p => p.Category)
                .Include(p => p.Photos)
                .FirstOrDefaultAsync(p => p.Id == updateProductDTO.Id);

            if (findProduct == null) return false;
            mapper.Map(updateProductDTO, findProduct);
            var oldPhotos = findProduct.Photos.ToList();
            foreach (var photo in oldPhotos)
            {
                imageManagementService.DeleteImageAsync(photo.ImageName);
            }
            context.Photos.RemoveRange(oldPhotos);

            if (updateProductDTO.Photos != null && updateProductDTO.Photos.Count > 0)
            {
                var newImagePaths = await imageManagementService.AddImageAsync(updateProductDTO.Photos, updateProductDTO.Name);

                var newPhotos = newImagePaths.Select(path => new Photo
                {
                    ImageName = path,
                    ProductId = updateProductDTO.Id
                }).ToList();

                await context.Photos.AddRangeAsync(newPhotos);
            }

            await context.SaveChangesAsync();
            return true;
        }
        public async Task<ReturnProductDTO> GetAllAsync(ProductParams productParams)
        {
            var query = context.Products.Include(m=>m.Category).Include(m=>m.Photos).AsNoTracking();
            // filtering by Word
            if(!string.IsNullOrEmpty(productParams.Search))
            {
                var searchWords =productParams.Search.Split(' ');
                query = query.Where(m=>searchWords.All(word=>
                m.Name.ToLower().Contains(word.ToLower())
                ||m.Description.ToLower().Contains(word.ToLower())));
            }
            // filtering by category ID
            if (productParams.CategoryId.HasValue)
            {
                query=query.Where(m=>m.CategoryId == productParams.CategoryId);
            }
            if(!string.IsNullOrEmpty(productParams.Sort))
            {
                query = productParams.Sort switch
                {
                    "PriceAce" => query.OrderBy(m => m.NewPrice),
                    "PriceDce" => query.OrderByDescending(m => m.NewPrice),
                    _ => query.OrderBy(m => m.Name),
                };
            }
            else
            {
                query = query.OrderBy(m => m.Name);
            }
            ReturnProductDTO returnProductDTO = new ReturnProductDTO();
            returnProductDTO.TotalCount=query.Count();
            query = query.Skip((productParams.PageSize) * (productParams.PageNumber - 1)).Take(productParams.PageSize);
            var list = await query.ToListAsync();
            returnProductDTO.products = mapper.Map<List<ProductDTO>>(list);
            return returnProductDTO;
        } 
    }
}
