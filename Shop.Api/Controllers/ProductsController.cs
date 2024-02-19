using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.Api.Dtos.ProductDtos;
using Shop.Core.Entities;
using Shop.Core.Repositories;

namespace Shop.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepository productRepository, IBrandRepository brandRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _brandRepository = brandRepository;
            _mapper = mapper;
        }

        [HttpPost("")]
        public IActionResult Create(ProductPostDto productPostDto)
        {
            if(!_brandRepository.IsExist(x=>x.Id == productPostDto.BrandId))
            {
                ModelState.AddModelError("BrandId", "BrandId is not correct!");
                return BadRequest(ModelState);  
            }

            Product product = _mapper.Map<Product>(productPostDto);

            _productRepository.Add(product);
            _productRepository.Commit();

            return StatusCode(201, new {Id = product.Id});
        }

        [HttpGet("all")]
        public ActionResult<List<ProductGetAllItemDto>> GetAll()
        {
            var data = _mapper.Map<List<ProductGetAllItemDto>>(_productRepository.GetAll(x=> true, "Brand"));

            return Ok(data);
        }

        [HttpPut("{id}")]
        public IActionResult Edit(int id, ProductPutDto productPutDto)
        {
            Product product = _productRepository.Get(x => x.Id == id);

            if (product == null)
                return NotFound();

            if (!_brandRepository.IsExist(x => x.Id == productPutDto.BrandId))
            {
                ModelState.AddModelError("BrandId", "BrandId is not correct!");
                return BadRequest(ModelState);
            }

            product.CostPrice = productPutDto.CostPrice;
            product.SalePrice = productPutDto.SalePrice;
            product.DiscountPercent = productPutDto.DiscountPercent;
            product.Name = productPutDto.Name;
            product.BrandId = productPutDto.BrandId;

            _productRepository.Commit();

            return NoContent();
        }

        [HttpGet("{id}")]
        public ActionResult<ProductGetDto> Get(int id)
        {
            Product product = _productRepository.Get(x => x.Id == id, "Brand");

            if (product == null)
                return NotFound();

            var data = _mapper.Map<ProductGetDto>(product);

            return Ok(data);
        }

    }
}
