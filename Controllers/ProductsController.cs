using Hplussport.API.Classes;
using Hplussport.API.Models;
using HPlusSport.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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

        public async Task<IActionResult> GetAllProducts([FromQuery] ProductQueryParameters queryParameters)
        {
            //change this to Iqueryable so that you can query on it
            //var products = await _shopContext.Products.ToListAsync();

            IQueryable<Product> products = _shopContext.Products;

            //means skip pages before the given page number and take the given page size items

            if (queryParameters.minprce != null && queryParameters.maxprice != null)
            {
                products = products.Where(
                    p => p.Price >= queryParameters.minprce &&
                         p.Price <= queryParameters.maxprice
                    );
            }
            if (!string.IsNullOrEmpty(queryParameters.sku))
            {
                products = products.Where(p => p.Sku == queryParameters.sku);
            }

            if (!string.IsNullOrEmpty(queryParameters.Name))
            {
                products = products.Where(

                p => p.Name.ToLower().Contains(queryParameters.Name.ToLower()));
            }

            if (!string.IsNullOrEmpty(queryParameters.SortBy))
            {

                if (typeof(Product).GetProperty(queryParameters.SortBy) != null)
                {

                    products = products.OrderByCustom(queryParameters.SortBy, queryParameters.SortOrder);
                }

            }

            products = products
                 .Skip(queryParameters.Size * queryParameters.Page - 1)
                 .Take(queryParameters.Size);

            return Ok(await products.ToArrayAsync());
        }

        [HttpGet, Route("{id:int}")]
        public async Task<IActionResult> GetProduct(int id) {

            var product = await _shopContext.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(_shopContext.Products.Find(id));
        }

        [HttpPost]
        public async Task<IActionResult> PostProduct(Product product)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            _shopContext.Products.Add(product);
            await _shopContext.SaveChangesAsync();

            return CreatedAtAction(
                "GetProduct",
                new { id = product.Id },
                product

                );

        }


        [HttpPut("id")]

        public async Task<IActionResult> PutProduct(int id, Product product)

        {
            if (!ModelState.IsValid) { return BadRequest(); }


            _shopContext.Entry(product).State = EntityState.Modified;
            try
            {

                await _shopContext.SaveChangesAsync();

            }
            catch (Exception DbUpdateConcurrencyException)
            {

                if (!_shopContext.Products.Any(p => p.Id == id))
                {

                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        [HttpDelete("{id}")]

        public async Task<ActionResult<Product>> DeleteProduct(int Id)
        {

            var product = await _shopContext.Products.FindAsync(Id);

            if(product == null)
            {

                return NotFound(Id);
            }

            _shopContext.Products.Remove(product);
            await _shopContext.SaveChangesAsync();
            return product;
        }


        [HttpPost]
        [Route("Delete")]

        public async Task<ActionResult> DeleteMultiple([FromQuery] int[] Id)
        {
            var products = new List<Product>();

            //if one item doesn't exist don't delete all.
            foreach(var id in Id)
            {
                var prod = await _shopContext.Products.FindAsync(id);

                if (prod == null)
                {
                    return NotFound();
                }
                products.Add(prod); 
            }


            _shopContext.Products.RemoveRange(products);
            await _shopContext.SaveChangesAsync();
            return Ok(products);
        }

        // add ef core and seed the data

    }
}
