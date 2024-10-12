using Microsoft.EntityFrameworkCore;
using ToDosProject.Domain.Entities;
using ToDosProject.Infraestructure.Context;

namespace ToDosProject.Tests.Helpers
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
            var user = await AddUserInContextAsync(context);
            var todoItem = new ToDo(0, "Fazer café")
            {
                UserId = user.Id,
            };

            context.ToDo.Add(todoItem);
            await context.SaveChangesAsync();
            return todoItem;
        }

        public static async Task<User> AddUserInContextAsync(AppDbContext context)
        {
            var user = new User()
            {
                Id = "1",
                Email = "userfound@todo.com"
            };

            context.User.Add(user);
            await context.SaveChangesAsync();
            return user;
        }
    }
}
