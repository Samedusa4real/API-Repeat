using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.Core.Entities;
using Shop.Core.Repositories;
using Shop.Services.Dtos.ProductDtos;
using Shop.Services.Interfaces;

namespace Shop.Api.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("")]
        public IActionResult Create([FromForm] ProductPostDto productPostDto)
        {
            return StatusCode(201, _productService.Create(productPostDto));
        }

        [HttpGet("all")]
        public ActionResult<List<ProductGetAllItemDto>> GetAll()
        {
            return Ok(_productService.GetAll());
        }

        [HttpPut("{id}")]
        public IActionResult Edit(int id,[FromForm] ProductPutDto productPutDto)
        {
            _productService.Edit(id, productPutDto);

            return NoContent();
        }

        [HttpGet("{id}")]
        public ActionResult<ProductGetDto> Get(int id)
        {
            return Ok(_productService.Get(id));
        }

    }
}
