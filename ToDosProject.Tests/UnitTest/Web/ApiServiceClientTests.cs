using RichardSzalay.MockHttp;
using System.Text.Json;
using ToDosProject.Domain.Entities;
using ToDosProject.Shared.Services;

namespace ToDosProject.Tests.UnitTest.Web
{
    public class ApiServiceClientTests
    {
        private readonly MockHttpMessageHandler _moqHttp;
        private readonly HttpClient _httpClient;
        private readonly ApiServiceClient _apiService;
        private const string FAKEURLAPI = "https://fakenaty";

        public ApiServiceClientTests()
        {
            _moqHttp = new MockHttpMessageHandler();
            _httpClient = _moqHttp.ToHttpClient();
            _httpClient.BaseAddress = new Uri(FAKEURLAPI);
            _apiService = new(_httpClient);
        }

        [Fact]
        public async Task GetToDosAsyncReturnsToDosOnSuccess()
        {
            var expectedToDos = new[]
            {
                new ToDo (1,"Task 1"),
                new ToDo (2,"Task 2")
            };

            _moqHttp.When($"{FAKEURLAPI}/todoitems")
                .Respond("application/json", JsonSerializer.Serialize(expectedToDos));

            var result = await _apiService.GetToDosAsync();

            Assert.NotNull(result);
            Assert.Equal(expectedToDos.Length, result.Length);
            Assert.Equal(expectedToDos[0].Title, result[0].Title);
            Assert.Equal(expectedToDos[1].Title, result[1].Title);
        }

        [Fact]
        public async Task GetToDosAsyncReturnsEmptyArrayOnError()
        {
            _moqHttp.When($"{FAKEURLAPI}/todoitems")
                .Respond(HttpStatusCode.InternalServerError);

            var result = await _apiService.GetToDosAsync();

            Assert.Empty(result);
        }

        [Fact]
        public async Task CreateToDosAsyncReturnsToDoOnSuccess()
        {
            ToDo toDo = new(1, "Almoçar");

            _moqHttp.When($"{FAKEURLAPI}/todoitems")
                .Respond("application/json", JsonSerializer.Serialize(toDo));

            var result = await _apiService.CreateToDosAsync(toDo);

            Assert.NotNull(result);
            Assert.IsType<ToDo?>(result);
        }

        [Fact]
        public async Task CreateToDosAsyncReturnsNullOnError()
        {
            ToDo toDo = new(1, "Almoçar");

            _moqHttp.When($"{FAKEURLAPI}/todoitems")
                .Respond(HttpStatusCode.InternalServerError);

            var result = await _apiService.CreateToDosAsync(toDo);

            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteToDoAsyncReturnsTrueOnSuccess()
        {
            _moqHttp.When($"{FAKEURLAPI}/todoitems/1")
                .Respond(HttpStatusCode.OK);

            var result = await _apiService.DeleteToDoAsync(1);

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteToDoAsyncReturnsFalseOnError()
        {
            _moqHttp.When($"{FAKEURLAPI}/todoitems/1")
                .Respond(HttpStatusCode.InternalServerError);

            var result = await _apiService.DeleteToDoAsync(1);

            Assert.False(result);
        }

        [Fact]
        public async Task UpdateToDoAsyncReturnsTrueOnSuccess()
        {
            ToDo toDo = new(1, "Almoçar");

            _moqHttp.When($"{FAKEURLAPI}/todoitems/1")
                .Respond(HttpStatusCode.OK);

            var result = await _apiService.UpdateToDoAsync(toDo);

            Assert.True(result);
        }

        [Fact]
        public async Task UpdateToDoAsyncReturnsFalseOnError()
        {
            ToDo toDo = new(1, "Almoçar");

            _moqHttp.When($"{FAKEURLAPI}/todoitems/1")
                .Respond(HttpStatusCode.InternalServerError);

            var result = await _apiService.UpdateToDoAsync(toDo);

            Assert.False(result);
        }

        [Fact]
        public async Task UnconcludeToDoAsyncReturnsTrueOnSuccess()
        {
            _moqHttp.When($"{FAKEURLAPI}/todoitems/Unconclude/1")
                .Respond(HttpStatusCode.OK);

            var result = await _apiService.UnconcludeToDoAsync(1);

            Assert.True(result);
        }

        [Fact]
        public async Task UnconcludeToDoAsyncReturnsFalseOnError()
        {
            _moqHttp.When($"{FAKEURLAPI}/todoitems/Unconclude/1")
                .Respond(HttpStatusCode.InternalServerError);

            var result = await _apiService.UnconcludeToDoAsync(1);

            Assert.False(result);
        }

        [Fact]
        public async Task ConcludeToDoAsyncReturnsTrueOnSuccess()
        {
            _moqHttp.When($"{FAKEURLAPI}/todoitems/Conclude/1")
                .Respond(HttpStatusCode.OK);

            var result = await _apiService.ConcludeToDoAsync(1);

            Assert.True(result);
        }

        [Fact]
        public async Task ConcludeToDoAsyncReturnsFalseOnError()
        {
            _moqHttp.When($"{FAKEURLAPI}/todoitems/Conclude/1")
                .Respond(HttpStatusCode.InternalServerError);

            var result = await _apiService.ConcludeToDoAsync(1);

            Assert.False(result);
        }
    }
}