using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using endavaRestApi.Data;
using endavaRestApi.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace endavaRestApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        private readonly IShopRepository _shopRepository;
        private readonly ICSVRepository _csvRepository;
        public ShopController(IShopRepository shopRepository, ICSVRepository csvRepository)
        {
            _shopRepository = shopRepository;
            _csvRepository = csvRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _shopRepository.Get();
        }

        [HttpPost("filter")]

        public async Task<ActionResult<Product>> Filter([FromBody] ProductFilter filter)
        {
            var products = await _shopRepository.Get();
            var results = products.Where(p =>
                    (filter.ProductCategory == null || p.ProductCategory == filter.ProductCategory) &&
                    (filter.ProductBrand == null || p.ProductBrand == filter.ProductBrand) &&
                    (filter.PriceMin == null || p.Price >= filter.PriceMin) &&
                    (filter.PriceMax == null || p.Price <= filter.PriceMax) &&
                    (filter.ProductSize == null || p.ProductSize == filter.ProductSize) &&
                    (filter.WeightMin == null || p.Weight >= filter.WeightMin) &&
                    (filter.WeightMax == null || p.Weight <= filter.WeightMax)
                );

            return Ok(results);
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            var createdUser = await _shopRepository.AddUser(user);

            return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);


        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            return await _shopRepository.Get(id);
        }
        [HttpPost("produkti")]
        public async Task<IActionResult> ImportProducts(IFormFile file)
        {
            await _csvRepository.VnesiCSV(file);
            return Ok();
        }
    }
}
