using Microsoft.AspNetCore.Http.HttpResults;
using ToDosProject.ApiService.MapGroups;
using ToDosProject.Domain.Entities;
using ToDosProject.Tests.UnitTest.Helpers;

namespace ToDosProject.Tests.UnitTest.ApiService.MapGroups
{
    public class ToDoItemEndnpoitsTest
    {
        [Fact]
        public async Task GetTodoReturnsNotFoundIfNotExists()
        {
            await using var context = new MockDB().CreateDbContext();

            var result = await ToDoItemEndnpoits.GetById(1, context);

            Assert.IsType<NotFound>(result);
        }

        [Fact]
        public async Task GetTodoReturnsOkIfExists()
        {
            await using var context = new MockDB().CreateDbContext();

            ToDo todoItem = await MockDB.AddTodoInContextAsync(context);

            var result = await ToDoItemEndnpoits.GetById(todoItem.Id, context);

            var okresult = Assert.IsType<Ok<ToDo>>(result);
            Assert.NotNull(okresult);
            Assert.Equal(okresult?.Value?.Id, todoItem.Id);
            Assert.Equal(okresult?.Value?.Title, todoItem.Title);
        }

        [Fact]
        public async Task GetAllReturnOk()
        {
            await using var context = new MockDB().CreateDbContext();

            var result = await ToDoItemEndnpoits.GetAll(context);

            Assert.IsType<Ok<ToDo[]>>(result);
        }

        [Fact]
        public async Task CreateToDoReturnsOk()
        {
            await using var context = new MockDB().CreateDbContext();

            var todo = new ToDo(0, "Fazer café");

            var result = await ToDoItemEndnpoits.Create(todo, context);

            Assert.IsType<Created<ToDo>>(result);
        }

        [Fact]
        public async Task UpdateReturnsNotFoundIfNotExist()
        {
            await using var context = new MockDB().CreateDbContext();

            var result = await ToDoItemEndnpoits.Update(2, new ToDo(2, "Fazer janta."), context);

            Assert.IsType<NotFound>(result);
        }

        [Fact]
        public async Task UpdateReturnsNoContenIftExist()
        {
            await using var context = new MockDB().CreateDbContext();

            ToDo todoItem = await MockDB.AddTodoInContextAsync(context);

            todoItem.Title = "Fazer janta";

            var result = await ToDoItemEndnpoits.Update(todoItem.Id, todoItem, context);

            Assert.IsType<NoContent>(result);

            var todoItemUpdated = await context.ToDo.FindAsync(todoItem.Id);

            Assert.NotNull(todoItemUpdated);
            Assert.Equal(todoItemUpdated.Id, todoItem.Id);
            Assert.Equal(todoItemUpdated.Title, todoItem.Title);
        }

        [Fact]
        public async Task DeleteReturnsNotFoundIfNotExist()
        {
            await using var context = new MockDB().CreateDbContext();

            var result = await ToDoItemEndnpoits.Delete(2, context);

            Assert.IsType<NotFound>(result);
        }

        [Fact]
        public async Task DeleteReturnsNoContenIftExist()
        {
            await using var context = new MockDB().CreateDbContext();

            ToDo todoItem = await MockDB.AddTodoInContextAsync(context);

            var result = await ToDoItemEndnpoits.Delete(todoItem.Id, context);

            Assert.IsType<NoContent>(result);

            var todoItemDeleted = await context.ToDo.FindAsync(todoItem.Id);

            Assert.Null(todoItemDeleted);
        }

        [Fact]
        public async Task ConcludeReturnsNotFoundIfNotExist()
        {
            await using var context = new MockDB().CreateDbContext();

            var result = await ToDoItemEndnpoits.Conclude(2, context);

            Assert.IsType<NotFound>(result);
        }

        [Fact]
        public async Task ConcludeReturnsNoContenIftExist()
        {
            await using var context = new MockDB().CreateDbContext();

            ToDo todoItem = await MockDB.AddTodoInContextAsync(context);

            var result = await ToDoItemEndnpoits.Conclude(todoItem.Id, context);

            Assert.IsType<NoContent>(result);

            var todoItemConcluded = await context.ToDo.FindAsync(todoItem.Id);

            Assert.NotNull(todoItemConcluded);
            Assert.True(todoItemConcluded.IsConcluded);
        }

        [Fact]
        public async Task UnconcludeReturnsNotFoundIfNotExist()
        {
            await using var context = new MockDB().CreateDbContext();

            var result = await ToDoItemEndnpoits.Unconclude(2, context);

            Assert.IsType<NotFound>(result);
        }

        [Fact]
        public async Task UnconcludeReturnsNoContenIftExist()
        {
            await using var context = new MockDB().CreateDbContext();

            ToDo todoItem = await MockDB.AddTodoInContextAsync(context);

            var result = await ToDoItemEndnpoits.Unconclude(todoItem.Id, context);

            Assert.IsType<NoContent>(result);

            var todoItemUnconcluded = await context.ToDo.FindAsync(todoItem.Id);

            Assert.NotNull(todoItemUnconcluded);
            Assert.False(todoItemUnconcluded.IsConcluded);
        }
    }
}
