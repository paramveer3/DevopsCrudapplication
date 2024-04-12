using DevopsCrudapplication.Controllers;
using DevopsCrudapplication.Data;
using DevopsCrudapplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DevopsCrudapplication.Tests
{
    public class InventoryControllerTests
    {



        [Fact]
        public async Task GetAllInventoryList_ReturnsListOfInventories()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<InventoryContext>()
                .UseInMemoryDatabase(databaseName: "GetAllInv_Database")
                .Options;
            using (var context = new InventoryContext(options))
            {
                context.Inventories.Add(new Inventory { Id = 1, Name = "Inventory 1" });
                context.Inventories.Add(new Inventory { Id = 2, Name = "Inventory 2" });
                context.SaveChanges();
            }

            using (var context = new InventoryContext(options))
            {
                var controller = new InventoryController(context);

                // Act
                var result = await controller.GetAllInventoryList();

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result.Result);
                var returnedBrands = Assert.IsAssignableFrom<IEnumerable<Inventory>>(okResult.Value);
                Assert.Equal(2, returnedBrands.Count());
            }
        }


        [Fact]
        public async Task AddToInventory_PostDataInInventory()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<InventoryContext>()
                .UseInMemoryDatabase(databaseName: "PostInvn_Database")
                .Options;
            using (var context = new InventoryContext(options))
            {
                var controller = new InventoryController(context);
                var newInv = new Inventory { Id = 1, Name = "Inventory 1" };

                // Act
                var result = await controller.AddToInventory(newInv);

                // Assert
                var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
                var returnedInventory = Assert.IsType<Inventory>(createdAtActionResult.Value);
                Assert.Equal(newInv.Id, returnedInventory.Id);
            }
        }

        [Fact]
        public async Task UpdateInventory_ValidIdAndInventory_ReturnsNoContent()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<InventoryContext>()
                .UseInMemoryDatabase(databaseName: "UpdateInv_Database")
                .Options;
            using (var context = new InventoryContext(options))
            {
                context.Inventories.Add(new Inventory { Id = 1, Name = "Inventory 1" });
                context.SaveChanges();
            }

            using (var context = new InventoryContext(options))
            {
                var controller = new InventoryController(context);
                var updatedInventory = new Inventory { Id = 1, Name = "Updated Inventory 1" };

                // Act
                var result = await controller.UpdateInventory(1, updatedInventory);

                // Assert
                Assert.IsType<NoContentResult>(result);
                Assert.Equal(updatedInventory.Name, context.Inventories.Find(1).Name);
            }
        }
        [Fact]
        public async Task DeleteInventory_ExistingId()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<InventoryContext>()
                .UseInMemoryDatabase(databaseName: "DeleteInv_Database")
                .Options;
            using (var context = new InventoryContext(options))
            {
                context.Inventories.Add(new Inventory { Id = 1, Name = "Brand 1" });
                context.SaveChanges();
            }

            using (var context = new InventoryContext(options))
            {
                var controller = new InventoryController(context);

                // Act
                var result = await controller.DeleteBrand(1);

                // Assert
                Assert.IsType<NoContentResult>(result);
                Assert.Null(context.Inventories.Find(1));
            }
        }


    }
}