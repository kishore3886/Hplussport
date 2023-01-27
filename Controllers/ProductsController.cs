using Hplussport.API.Classes;
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

        public async Task<IActionResult> GetAllProducts([FromQuery]ProductQueryParameters queryParameters )
        {
            //change this to Iqueryable so that you can query on it
            //var products = await _shopContext.Products.ToListAsync();

            IQueryable<Product> products =  _shopContext.Products;

            //means skip pages before the given page number and take the given page size items

            if (queryParameters.minprce != null && queryParameters.maxprice!= null)
            {
                products = products.Where(
                    p => p.Price >= queryParameters.minprce &&
                         p.Price <= queryParameters.maxprice
                    ); 
            }
            if(!string.IsNullOrEmpty(queryParameters.sku))
            {
                products=products.Where(p=>p.Sku == queryParameters.sku);
            }
            products = products
                 .Skip(queryParameters.Size * queryParameters.Page-1 )
                 .Take(queryParameters.Size);

            return Ok(await products.ToArrayAsync());
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
