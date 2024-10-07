using Bunit;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.DataProtection;
using ToDosProject.Domain;
using ToDosProject.Web;
using ToDosProject.Web.Components.Pages;

namespace ToDosProject.Tests;

public class WebTests : TestContext, IDisposable
{
    private readonly DistributedApplication app = null!;
    private readonly HttpClient httpClientWeb = null!;
    private readonly HttpClient httpClientApi = null!;
    public WebTests()
    {
        var appHost = DistributedApplicationTestingBuilder.CreateAsync<Projects.ToDosProject_AppHost>().Result;
        appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddStandardResilienceHandler();
        });

        app = appHost.BuildAsync().Result;
        var resourceNotificationService = app.Services.GetRequiredService<ResourceNotificationService>();
        app.StartAsync().Wait();

        resourceNotificationService.WaitForResourceAsync(AppConfiguration.DATABASE, KnownResourceStates.Running).Wait();
        resourceNotificationService.WaitForResourceAsync(AppConfiguration.CACHE, KnownResourceStates.Running).Wait();
        resourceNotificationService.WaitForResourceAsync(AppConfiguration.API, KnownResourceStates.Running).Wait();
        resourceNotificationService.WaitForResourceAsync(AppConfiguration.WEB, KnownResourceStates.Running).Wait();
        resourceNotificationService.WaitForResourceAsync(AppConfiguration.MIGRATION, KnownResourceStates.Finished).Wait();

        httpClientWeb = app.CreateHttpClient(AppConfiguration.WEB);
        httpClientApi = app.CreateHttpClient(AppConfiguration.API);


        ApiServiceClient apiServiceClient = new(httpClientApi);

        Services.AddSingleton(apiServiceClient);
    }

    [Fact]
    public async Task GetWebResourceRootReturnsOkStatusCode()
    {
        // Act
        var response = await httpClientWeb.GetAsync("/");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetWebResourceTodosReturnsOkStatusCode()
    {
        var cut = RenderComponent<ToDoPage>();

        var p = cut.Find("#p-loading");

        Assert.True(cut.Instance.toDos == null || p.InnerHtml == "<em>Loading...</em>");

        cut.WaitForElement("#input-createtodo", TimeSpan.FromMinutes(1));

        var input = cut.Find("#input-createtodo");

        input.Change("Nova tarefa");    

        await cut.InvokeAsync(() => input.KeyUp(new KeyboardEventArgs { Key = "Enter"}));
        
        cut.WaitForElement("#tr-0", TimeSpan.FromSeconds(30));
        
        var inputTitle = cut.Find("#input-title-0");

        inputTitle.Change("Nova tarefa 1");    

        var checkbox = cut.Find("#input-isconcluded-0");

        checkbox.Click();

        cut.WaitForState(() => cut.Instance.toDos![0].IsConcluded == true,TimeSpan.FromSeconds(10));

        cut = RenderComponent<ToDoPage>();

        cut.WaitForState(() => cut?.Instance?.toDos != null, TimeSpan.FromSeconds(30));

        Assert.Equal("Nova tarefa 1", cut.Instance.toDos![0].Title);
        Assert.True(cut.Instance.toDos![0].IsConcluded);

        cut = RenderComponent<ToDoPage>();

        cut.WaitForState(() => cut?.Instance?.toDos != null, TimeSpan.FromSeconds(30));

        checkbox = cut.Find("#input-isconcluded-0");

        checkbox.Click();

        cut.WaitForState(() => cut.Instance.toDos![0].IsConcluded == false, TimeSpan.FromSeconds(10));

        cut = RenderComponent<ToDoPage>();

        cut.WaitForState(() => cut?.Instance?.toDos != null, TimeSpan.FromSeconds(30));

        Assert.False(cut.Instance.toDos![0].IsConcluded);

        checkbox = cut.Find("#input-delete-0");

        checkbox.Click();

        cut.WaitForState(() => cut?.Instance?.toDos.Count() == 0, TimeSpan.FromSeconds(30));
    }

    protected override void Dispose(bool disposing)
    {
        app.StopAsync().Wait();
    }
}
