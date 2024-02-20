using AutoMapper;
using Shop.Core.Entities;
using Shop.Core.Repositories;
using Shop.Services.Dtos.BrandDtos;
using Shop.Services.Dtos.CommonDtos;
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
                throw new Exception();
            
            var entity = _mapper.Map<Brand>(postDto);

            _brandRepository.Add(entity);
            _brandRepository.Commit();

            return new CreatedDto { Id = entity.Id };
        }

        public void Delete(int id)
        {
            var entity = _brandRepository.Get(x => x.Id == id);

            if (entity != null)
                throw new Exception();

            _brandRepository.Remove(entity);
            _brandRepository.Commit();
        }

        public void Edit(int id, BrandPutDto putDto)
        {
            throw new NotImplementedException();
        }

        public List<BrandGetAllItemDto> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
