﻿using AutoMapper;
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

namespace Shop.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IBrandRepository _brandRepository;

        public ProductService(IProductRepository productRepository, IMapper mapper, IBrandRepository brandRepository)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _brandRepository = brandRepository;
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
            throw new NotImplementedException();
        }

        public void Edit(int id, ProductPutDto putDto)
        {
            throw new NotImplementedException();
        }

        public ProductGetDto Get(int id)
        {
            throw new NotImplementedException();
        }

        public List<ProductGetAllItemDto> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
