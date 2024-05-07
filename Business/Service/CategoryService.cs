using Business.Core;
using Business.Interface;
using Entity.Entities;
using Microsoft.AspNetCore.Identity;
using Shopping.Business.Core;
using Shopping.Data.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Service
{
    public class CategoryService:ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryDto> Create(CategoryResponse res)
        {
            Guid id = Guid.NewGuid();
            var response = new CategoryDto();
            while (await _categoryRepository.GetByIdAsync(id) != null)
            {
                id = Guid.NewGuid();
            }
            
            var newcategory = new Category
            {
                Id = id,
                Name = res.Name,
            };
            await _categoryRepository.AddAsync(newcategory);
            response.DeveloperMessage = "Category Created Succesfull";
            response.Category = newcategory;
            response.Message = "Success";
            response.Status = 200;
            return response;
        }

        public async Task<BaseDto> Delete(Guid Id)
        {
            var category = await _categoryRepository.GetByIdAsync(Id);
            var response = new BaseDto();
            if (category == null)
            {
                response.DeveloperMessage = "Category Not Found";
                response.Message = "Not Found";
                response.Status = 404;
                return response;
            }
            await _categoryRepository.DeleteAsync(Id);
            response.DeveloperMessage = "Category removed successful";
            response.Message = "Success";
            response.Status = 200;
            return response;
        }

        public async Task<CategoryListDto> GetAll(int page)
        {
            var categories = await _categoryRepository.GetAllAsync(page);
            var res = new CategoryListDto
            {
                Categories = categories,
                Message = "Success",
                Status = 200
            };
            return res;
        }

        public async Task<CategoryDto> Get(Guid Id)
        {
            var response = new CategoryDto();
            var category = await _categoryRepository.GetByIdAsync(Id);
            if (category == null)
            {
                response.DeveloperMessage = "Category Not Found";
                response.Message = "Not Found";
                response.Status = 404;
                return response;
            }
            response.Message = "Success";
            response.Category = category;
            response.Status = 200;
            return response;
        }
        public async Task<CategoryDto> Update(Guid Id, CategoryResponse res)
        {
            var response = new CategoryDto();
            var category = await _categoryRepository.GetByIdAsync(Id);
            if (category == null)
            {
                response.DeveloperMessage = "Category Not Found";
                response.Message = "Not Found";
                response.Status = 404;
                return response;
            }
            category.Name = res.Name;
            await _categoryRepository.UpdateAsync(Id, category);
            response.DeveloperMessage = "Update Success";
            response.Message = "Success";
            response.Status = 200;
            response.Category = category;
            return response;
        }
    }
}
