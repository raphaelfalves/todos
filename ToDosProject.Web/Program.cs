using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;
using ToDosProject.Domain.Constants;
using ToDosProject.ServiceDefaults;
using ToDosProject.Web;
using ToDosProject.Web.Components;
using ToDosProject.Web.Handler;
using ToDosProject.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents(options =>
{
	options.DetailedErrors = builder.Environment.IsDevelopment();
});

// Add handlers to the authentication
builder.Services.AddScoped<CookieHandler>();
builder.Services.AddScoped<AuthenticationStateProvider, CookieAuthenticationStateProvider>();
builder.Services.AddScoped(x => (CookieAuthenticationStateProvider)x.GetRequiredService<AuthenticationStateProvider>());

builder.Services.AddHttpClient<ApiServiceClient>(
	client =>
	{
		// This URL uses "https+http://" to indicate HTTPS is preferred over HTTP.
		// Learn more about service discovery scheme resolution at https://aka.ms/dotnet/sdschemes.
		client.BaseAddress = new($"https+http://{AppConfiguration.API}");
	})
	.AddHttpMessageHandler<CookieHandler>();

builder.Services.AddMudServices();

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
	.AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();

app.Run();
