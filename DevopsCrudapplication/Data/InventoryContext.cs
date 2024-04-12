using DevopsCrudapplication.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DevopsCrudapplication.Data
{
    public class InventoryContext : DbContext
    {
        public InventoryContext(DbContextOptions<InventoryContext> options) : base(options)
        {

        }

        public DbSet<Inventory> Inventories { get; set; }
    }
}
