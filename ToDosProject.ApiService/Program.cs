using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PainelPartner.Models;
using ToDosProject.ApiService.Extensions;
using ToDosProject.ApiService.MapGroups;
using ToDosProject.Domain.Constants;
using ToDosProject.Domain.Entities;
using ToDosProject.Domain.Services;
using ToDosProject.Infraestructure.Context;
using ToDosProject.ServiceDefaults;

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

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<UserContextService>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Add services to the container.
builder.Services.AddProblemDetails();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<User>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddErrorDescriber<CustomIdentityErrorDescriber>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Configure the HTTP request pipeline.
app.UseExceptionHandler();

var todoItems = app.MapGroup("/todoitems").RequireAuthorization();

todoItems.MapGet("/", ToDoItemEndnpoits.GetAll);

todoItems.MapGet("/{id}", ToDoItemEndnpoits.GetById);

todoItems.MapPost("/", ToDoItemEndnpoits.Create);

todoItems.MapPut("/{id}", ToDoItemEndnpoits.Update);

todoItems.MapDelete("/{id}", ToDoItemEndnpoits.Delete);

todoItems.MapPost("/Conclude/{id}", ToDoItemEndnpoits.Conclude);

todoItems.MapPost("/Unconclude/{id}", ToDoItemEndnpoits.Unconclude);

app.MapIdentityApi<User>();

app.MapDefaultEndpoints();

app.Services.InitializeDb();

app.Run();
