using Shop.Services.Dtos.BrandDtos;
using Shop.Services.Dtos.CommonDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Services.Interfaces
{
    public interface IBrandService
    {
        CreatedDto Create(BrandPostDto postDto);
        void Edit(int id, BrandPutDto putDto);
        List<BrandGetAllItemDto> GetAll();
        void Delete(int id);
        BrandGetDto Get(int id);
    }
}
