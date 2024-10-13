using ToDosProject.ServiceDefaults;
using ToDosProject.Shared.Extendions;
using ToDosProject.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents(options =>
{
    options.DetailedErrors = builder.Environment.IsDevelopment();
});

// Add handlers to the authentication
builder.Services.AddServices();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(ToDosProject.Shared._Imports).Assembly); ;

app.MapDefaultEndpoints();

app.Run();
