using Business.Core;
using Business.Interface;
using Entity.Entities;
using Microsoft.IdentityModel.Tokens;
using Shopping.Business.Core;
using Shopping.Data.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Service
{
    public class ProductService:IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<ProductDto> Create(ProductResponse res)
        {
            Guid id = Guid.NewGuid();
            var response = new ProductDto();
            var category = await _categoryRepository.GetByIdAsync(res.CategoryId);
            if (category == null)
            {
                response.DeveloperMessage = "Category Not Found";
                response.Message = "Not Found";
                response.Status = 404;
            }
            while (await _productRepository.GetByIdAsync(id) != null)
            {
                id = Guid.NewGuid();
            }
            if (res.Image == null || res.Image.Length == 0)
            {
                response.DeveloperMessage = "Image is required";
                response.Message = "Bad Request";
                response.Status = 400;
            }
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Upload/Product_Images");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + res.Image.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await res.Image.CopyToAsync(fileStream);
            }
            var imageUrl = "https://localhost:7155/Upload/Product_Images/" + uniqueFileName;
            var newproduct = new Product
            {
                Id = id,
                Name = res.Name,
                CategoryId = res.CategoryId,
                ImageUrl = imageUrl,

            };
            await _productRepository.AddAsync(newproduct);
            response.DeveloperMessage = "Product Created Succesfull";
            response.Product = newproduct;
            response.Message = "Success";
            response.Status = 200;
            return response;
        }

        public async Task<BaseDto> Delete(Guid Id)
        {
            var product = await _productRepository.GetByIdAsync(Id);
            var response = new BaseDto();
            if (product == null)
            {
                response.DeveloperMessage = "Product Not Found";
                response.Message = "Not Found";
                response.Status = 404;
                return response;
            }
            string imagesFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Upload/Product_Images");

            if (!string.IsNullOrEmpty(product.ImageUrl))
            {
                var imagePath = Path.Combine(imagesFolderPath, Path.GetFileName(product.ImageUrl));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
            await _productRepository.DeleteAsync(Id);
            response.DeveloperMessage = "Product removed successful";
            response.Message = "Success";
            response.Status = 200;
            return response;
        }

        public async Task<ProductAllDto> GetAll(int page,Guid? categoryid,string keyword)
        {
            var products = await _productRepository.GetAllAsync(page,categoryid,keyword);
            var res = new ProductAllDto()
            {
                Products = products,
                Message = "Success",
                Status = 200
            };
            return res;
        }

        public async Task<ProductDto> Get(Guid Id)
        {
            var response = new ProductDto();
            var product = await _productRepository.GetByIdAsync(Id);
            if (product == null)
            {
                response.DeveloperMessage = "product Not Found";
                response.Message = "Not Found";
                response.Status = 404;
                return response;
            }
            response.Message = "Success";
            response.Product = product;
            response.Status = 200;
            return response;
        }
        public async Task<ProductDto> Update(Guid Id, ProductUpdateResponse res)
        {
            var response = new ProductDto();
            var product = await _productRepository.GetByIdAsync(Id);
            if (product == null)
            {
                response.DeveloperMessage = "product Not Found";
                response.Message = "Not Found";
                response.Status = 404;
                return response;
            }
            if (res.CategoryId.HasValue)
            {
                var category = await _categoryRepository.GetByIdAsync(res.CategoryId.Value);
                if (category == null)
                {
                    response.DeveloperMessage = "Category Not Found";
                    response.Message = "Not Found";
                    response.Status = 404;
                }
                product.CategoryId = res.CategoryId.Value;
            }
            if (res.Image != null && res.Image.Length != 0)
            {
                string imagesFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Upload/Product_Images");

                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    var imagePath = Path.Combine(imagesFolderPath, Path.GetFileName(product.ImageUrl));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Upload/Product_Images");

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + res.Image.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await res.Image.CopyToAsync(fileStream);
                    }
                    product.ImageUrl = "https://localhost:7155/Upload/Product_Images/" + uniqueFileName;
                }
            }
            if (!res.Name.IsNullOrEmpty())
            {
                product.Name = res.Name;
            }
            await _productRepository.UpdateAsync(Id, product);
            response.DeveloperMessage = "Product update Succesfull";
            response.Product = product;
            response.Message = "Success";
            response.Status = 200;
            return response;
        }
    }
}
