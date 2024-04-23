using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VerivoxTariffComparisor.Data;
using VerivoxTariffComparisor.Model;

namespace VerivoxTariffComparisor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TariffProviderController(TariffDbContext _context) : ControllerBase
    {
        

        // GET: api/TariffProvider
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetTariffProvider()
        {
            return await _context.TariffProvider.ToListAsync();
        }

        // GET: api/TariffProvider/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(string id)
        {
            var product = await _context.TariffProvider.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/TariffProvider/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(string id, Product product)
        {
            if (id != product.name)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // POST: api/TariffProvider
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.TariffProvider.Add(product);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ProductExists(product.name))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetProduct", new { id = product.name }, product);
        }

        // DELETE: api/TariffProvider/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var product = await _context.TariffProvider.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.TariffProvider.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(string id)
        {
            return _context.TariffProvider.Any(e => e.name == id);
        }
    }
}
