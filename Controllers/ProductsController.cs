using Hplussport.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
namespace Hplussport.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly ShopContext _shopContext;

        public ProductsController(ShopContext shopContext)
        {
            _shopContext = shopContext;

            _shopContext.Database.EnsureCreated();
        }

        [HttpGet]
        //public IEnumerable<Product> GetAllProducts() {

        //    return _shopContext.Products.ToArray();
        //}

        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _shopContext.Products.ToListAsync();

            return Ok(products);
        }

        [HttpGet,Route("{id:int}")]
        public async Task<IActionResult> GetProduct(int id) { 
        
            var product = await _shopContext.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            
            return Ok(_shopContext.Products.Find(id));
        } 


       // add ef core and seed the data

    }
}
