using EFCoreSqlServer.Context;
using EFCoreSqlServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EFCoreSqlServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _productsDbContext;
        public ProductsController(ApplicationDbContext productsDbContext)
        {
            _productsDbContext = productsDbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct()
        {
            return Ok(await _productsDbContext.Products.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Create([FromBody] Product product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _productsDbContext.Products.AddAsync(product);

            await _productsDbContext.SaveChangesAsync();

            return Ok(product);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> Update(int id, [FromBody] Product productFromJson)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = await _productsDbContext.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            product.Name = productFromJson.Name;
            product.Price = productFromJson.Price;
            product.Description = productFromJson.Description;

            await _productsDbContext.SaveChangesAsync();

            return Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> Delete(int id)
        {
            var product = await _productsDbContext.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }
            
            _productsDbContext.Remove(product);

            await _productsDbContext.SaveChangesAsync();

            return Ok(product);
        }
    }
}
