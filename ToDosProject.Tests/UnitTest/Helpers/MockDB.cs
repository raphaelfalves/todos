using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDosProject.Domain;
using ToDosProject.Infraestructure;

namespace ToDosProject.Tests.UnitTest.Helpers
{
    public class MockDB : IDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase($"InMemoryTestDb-{DateTime.Now.ToFileTimeUtc()}")
                .Options;

            return new AppDbContext(options);
        }

        public static async Task<ToDo> AddTodoInContextAsync(AppDbContext context)
        {
            var todoItem = new ToDo(0, "Fazer café");

            context.ToDo.Add(todoItem);
            await context.SaveChangesAsync();
            return todoItem;
        }
    }
}
