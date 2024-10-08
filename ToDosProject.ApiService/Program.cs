using Microsoft.EntityFrameworkCore;
using ToDosProject.ApiService.MapGroups;
using ToDosProject.Domain;
using ToDosProject.Infraestructure;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

builder.AddSqlServerDbContext<AppDbContext>(AppConfiguration.SQL, configureDbContextOptions: options =>
{
    options.UseSqlServer(sqlServerOptions =>
    {
        sqlServerOptions.MigrationsAssembly("ToDosProject.Infraestructure");
    });
});

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Add services to the container.
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

var todoItems = app.MapGroup("/todoitems");

todoItems.MapGet("/", ToDoItemEndnpoits.GetAll);

todoItems.MapGet("/{id}", ToDoItemEndnpoits.GetById);

todoItems.MapPost("/", ToDoItemEndnpoits.Create);

todoItems.MapPut("/{id}", ToDoItemEndnpoits.Update);

todoItems.MapDelete("/{id}", ToDoItemEndnpoits.Delete);

todoItems.MapPost("/Conclude/{id}", ToDoItemEndnpoits.Conclude);

todoItems.MapPost("/Unconclude/{id}", ToDoItemEndnpoits.Unconclude);

app.MapDefaultEndpoints();

app.Run();
