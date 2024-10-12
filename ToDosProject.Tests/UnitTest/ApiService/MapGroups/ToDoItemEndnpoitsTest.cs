using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using System.Security.Claims;
using ToDosProject.ApiService.MapGroups;
using ToDosProject.Domain.Entities;
using ToDosProject.Domain.Exceptions;
using ToDosProject.Web.Services;
using ToDosProject.Web;
using System.Collections.Generic;
using ToDosProject.Tests.Helpers;
using Microsoft.AspNetCore.Mvc;
using IdentityModel.OidcClient;

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

            var user = await MockDB.AddUserInContextAsync(context);

            Mock<IHttpContextAccessor> httpContextAccessorMock = MockAcessor.CreateAccessorAuthenticated(user);

            var result = await ToDoItemEndnpoits.GetAll(context, httpContextAccessorMock.Object);

            Assert.IsType<Ok<ToDo[]>>(result);
        }

        [Fact]
        public async Task GetAllReturnOnlyToDosOfAuthenticatedUser()
        {
            await using var context = new MockDB().CreateDbContext();

            await MockDB.InitializeDbAsync(context);

            var user = context.User.Find("1");

            Mock<IHttpContextAccessor> httpContextAccessorMock = MockAcessor.CreateAccessorAuthenticated(user!);

            var result = await ToDoItemEndnpoits.GetAll(context, httpContextAccessorMock.Object);

            var okResult = Assert.IsType<Ok<ToDo[]>>(result);
            Assert.NotNull(okResult.Value);
            Assert.NotEmpty(okResult.Value);
            Assert.All(okResult.Value, item => Assert.Equal(user!.Id, item.UserId));
        }

        [Fact]
        public async Task CreateToDoReturnsOk()
        {
            await using var context = new MockDB().CreateDbContext();

            var user = await MockDB.AddUserInContextAsync(context);

            Mock<IHttpContextAccessor> httpContextAccessorMock = MockAcessor.CreateAccessorAuthenticated(user);

            var todo = new ToDo(0, "Fazer café")
            {
                UserId = user.Id
            };

            var result = await ToDoItemEndnpoits.Create(todo, context, httpContextAccessorMock.Object);

            Assert.IsType<Created<ToDo>>(result);
        }

        [Fact]
        public async Task CreateToDoReturnsUnauthorized()
        {
            await using var context = new MockDB().CreateDbContext();

            var httpContextAccessorMock = MockAcessor.CreateAcessorUnauthenticated();

            var todo = new ToDo(0, "Fazer café");

            var response = await ToDoItemEndnpoits.Create(todo, context, httpContextAccessorMock.Object);

            Assert.IsType<UnauthorizedHttpResult>(response);
        }

        [Fact]
        public async Task GetUserIdThrowUserNotFoundExpceptionWhenUserNotFound()
        {
            //Arrage
            await using var context = new MockDB().CreateDbContext();

            var httpContextAccessorMock = MockAcessor.CreateAcessorUnauthenticated();

            //Act
            var task = ToDoItemEndnpoits.GetUserId(context, httpContextAccessorMock.Object);

            //Assert
            await Assert.ThrowsAsync<UserNotFoundExpception>(() => task);
        }

        [Fact]
        public async Task GetUserIdReturnsUserId()
        {
            //Arrage
            await using var context = new MockDB().CreateDbContext();

            var user = await MockDB.AddUserInContextAsync(context);

            var httpContextAccessorMock = MockAcessor.CreateAccessorAuthenticated(user);

            //Act
            var response = await ToDoItemEndnpoits.GetUserId(context, httpContextAccessorMock.Object);

            //Assert
            Assert.Equal(user.Id, response);
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
