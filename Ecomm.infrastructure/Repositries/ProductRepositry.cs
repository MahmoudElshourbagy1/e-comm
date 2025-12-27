using Ecom.Core.DTO;
using Ecom.Core.Entites.Product;
using Ecom.Core.Interfaces;
using Ecomm.infrastructure.Data;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Ecom.Core.Services;
using Microsoft.EntityFrameworkCore;

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

    }
}
