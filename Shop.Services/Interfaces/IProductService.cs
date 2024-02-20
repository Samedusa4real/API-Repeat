using Shop.Services.Dtos.CommonDtos;
using Shop.Services.Dtos.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Services.Interfaces
{
    public interface IProductService
    {
        CreatedDto Create(ProductPostDto postDto);
        void Edit(int id, ProductPutDto putDto);
        void Delete(int id);
        ProductGetDto Get(int id);
        List<ProductGetAllItemDto> GetAll();

    }
}
