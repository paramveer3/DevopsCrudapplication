using DevopsCrudapplication.Data;
using DevopsCrudapplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevopsCrudapplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {

        private readonly InventoryContext _context;

        public InventoryController(InventoryContext context)
        {
            _context = context;
        }


        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Inventory>>> GetAllInventoryList()
        {
            var myinventories = await _context.Inventories.ToListAsync();
            return Ok(myinventories);
        }


        [HttpPost("Add")]
        public async Task<ActionResult<Inventory>> AddToInventory(Inventory inventory)
        {
            _context.Inventories.Add(inventory);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetInventoryById), new { id = inventory.Id }, inventory);
        }


        [HttpGet("Get/{id}")]
        public async Task<ActionResult<Inventory>> GetInventoryById(int id)
        {
            var myinventory = await _context.Inventories.FindAsync(id);
            if (myinventory == null)
            {
                return NotFound();
            }
            return myinventory;
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            var brand = await _context.Inventories.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }

            _context.Inventories.Remove(brand);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AvailableInventory(int id)
        {
            return _context.Inventories.Any(e => e.Id == id);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateInventory(int id, Inventory inventory)
        {
            if (id != inventory.Id)
            {
                return BadRequest();
            }

            _context.Entry(inventory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AvailableInventory(id))
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
    }
}
