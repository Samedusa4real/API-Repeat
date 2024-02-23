using AutoMapper;
using Shop.Core.Entities;
using Shop.Core.Repositories;
using Shop.Services.Dtos.BrandDtos;
using Shop.Services.Dtos.CommonDtos;
using Shop.Services.Exceptions;
using Shop.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Services.Implementations
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;

        public BrandService(IBrandRepository brandRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }
        public CreatedDto Create(BrandPostDto postDto)
        {
            if (_brandRepository.IsExist(x => x.Name == postDto.Name))
                throw new RestException(System.Net.HttpStatusCode.BadRequest,"Name", "BrandName is already taken!");
            
            var entity = _mapper.Map<Brand>(postDto);

            _brandRepository.Add(entity);
            _brandRepository.Commit();

            return new CreatedDto { Id = entity.Id };
        }

        public void Delete(int id)
        {
            var entity = _brandRepository.Get(x => x.Id == id);

            if (entity == null)
                throw new RestException(System.Net.HttpStatusCode.NotFound, "Missing information");

            _brandRepository.Remove(entity);
            _brandRepository.Commit();
        }

        public void Edit(int id, BrandPutDto putDto)
        {
            var entity = _brandRepository.Get(x => x.Id == id);

            if (entity != null)
                throw new RestException(System.Net.HttpStatusCode.NotFound, "Missing information");

            if (entity.Name != putDto.Name && _brandRepository.IsExist(x => x.Name == putDto.Name))
                throw new RestException(System.Net.HttpStatusCode.BadRequest, "Name", "BrandName is already taken!");

            entity.Name = putDto.Name;
            _brandRepository.Commit();
        }

        public BrandGetDto Get(int id)
        {
            var entity = _brandRepository.Get(x => x.Id == id, "Products");

            if (entity == null)
                throw new RestException(System.Net.HttpStatusCode.NotFound, "Missing information");

            return _mapper.Map<BrandGetDto>(entity);
        }

        public List<BrandGetAllItemDto> GetAll()
        {
            var entities = _brandRepository.GetAll(x => true);

            return _mapper.Map<List<BrandGetAllItemDto>>(entities);
        }
    }
}
