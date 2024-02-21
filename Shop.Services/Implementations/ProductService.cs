using AutoMapper;
using Microsoft.AspNetCore.Http;
using Shop.Core.Entities;
using Shop.Core.Repositories;
using Shop.Services.Dtos.CommonDtos;
using Shop.Services.Dtos.ProductDtos;
using Shop.Services.Exceptions;
using Shop.Services.Helpers;
using Shop.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Shop.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IBrandRepository _brandRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductService(IProductRepository productRepository, IMapper mapper, IBrandRepository brandRepository, IHttpContextAccessor httpContextAccessor)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _brandRepository = brandRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public CreatedDto Create(ProductPostDto postDto)
        {
            List<RestExceptionError> errors = new List<RestExceptionError>();

            if (!_brandRepository.IsExist(x => x.Id == postDto.BrandId))
                errors.Add(new RestExceptionError("BrandId", "Brand is not valid!"));

            if (_productRepository.IsExist(x => x.Name == postDto.Name))
                errors.Add(new RestExceptionError("Name", "Name is already exist!"));

            if (errors.Count > 0) throw new RestException(System.Net.HttpStatusCode.BadRequest, errors);

            var entity = _mapper.Map<Product>(postDto);

            string rootPath = Directory.GetCurrentDirectory() + "/wwwroot";
            entity.ImageName = FileManager.Save(postDto.ImageFile, rootPath, "uploads/products");

            _productRepository.Add(entity);
            _productRepository.Commit();

            return new CreatedDto { Id = entity.Id };
        }

        public void Delete(int id)
        {
            var entity = _productRepository.Get(x => x.Id == id);

            if (entity == null)
                throw new RestException(System.Net.HttpStatusCode.NotFound, "Product is not exist!");

            _productRepository.Remove(entity);
            _productRepository.Commit();

            string rootPath = Directory.GetCurrentDirectory() + "/wwwroot";
            FileManager.Delete(rootPath, "uploads/products", entity.ImageName);
        }

        public void Edit(int id, ProductPutDto putDto)
        {
            var entity = _productRepository.Get(x => x.Id == id);

            if (entity == null)
                throw new RestException(System.Net.HttpStatusCode.NotFound, "Product is not exist!");

            List<RestExceptionError> errors = new List<RestExceptionError>();

            if (!_brandRepository.IsExist(x => x.Id == putDto.BrandId))
                errors.Add(new RestExceptionError("BrandId", "Brand is not valid!"));

            if (putDto.Name != entity.Name && _productRepository.IsExist(x => x.Name == putDto.Name))
                errors.Add(new RestExceptionError("Name", "Name is already exist!"));

            entity.Name = putDto.Name;
            entity.CostPrice = putDto.CostPrice;
            entity.SalePrice = putDto.SalePrice;
            entity.DiscountPercent = putDto.DiscountPercent;
            entity.BrandId = putDto.BrandId;

            string removableFileName = null;
            string rootPath = Directory.GetCurrentDirectory() + "/wwwroot";

            if (putDto.ImageFile != null)
            {
                removableFileName = entity.ImageName;
                entity.ImageName = FileManager.Save(putDto.ImageFile, rootPath, "uploads/products");
            }

            _productRepository.Commit();

            if (removableFileName != null)
                FileManager.Delete(rootPath, "uploads/products", removableFileName);
        }

        public ProductGetDto Get(int id)
        {
            var entity = _productRepository.Get(x => x.Id == id, "Brand");

            if (entity == null)
                throw new RestException(System.Net.HttpStatusCode.NotFound, "Product is not exist!");

            return _mapper.Map<ProductGetDto>(entity);
        }

        public List<ProductGetAllItemDto> GetAll()
        {
            var entities = _productRepository.GetAll(x => true, "Brand");

            return _mapper.Map<List<ProductGetAllItemDto>>(entities);
        }
    }
}
