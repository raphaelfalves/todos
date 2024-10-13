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

        public static async Task InitializeDbAsync(AppDbContext context)
        {
            User[] users = [new User()
            {
                Id = "1",
                Email = "userfound@todo.com"
            },new User()
            {
                Id = "2",
                Email = "usernotfound@todo.com"
            }];

            context.User.AddRange(users);

            ToDo[] todoItems =
            [
                new ToDo(0, "Fazer café")
                {
                    User = users[0],
                },
                new ToDo(0, "Fazer chá")
                {
                    User = users[1],
                }
            ];

            context.ToDo.AddRange(todoItems);
            await context.SaveChangesAsync();
        }
    }
}
