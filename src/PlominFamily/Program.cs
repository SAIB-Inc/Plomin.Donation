using MudBlazor.Services;
using PlominFamily.Components;
using PlominFamily.Services;
using Utxorpc.Sdk;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMudServices();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddSingleton(new QueryServiceClient(
    builder.Configuration["UtxoRpcUrl"]!,
    new Dictionary<string, string>
    {
        { "dmtr-api-key", builder.Configuration["UtxoRpcApiKey"]! }
    }
));
builder.Services.AddSingleton(new SyncServiceClient(
    builder.Configuration["UtxoRpcUrl"]!,
    new Dictionary<string, string>
    {
        { "dmtr-api-key", builder.Configuration["UtxoRpcApiKey"]! }
    }
));
builder.Services.AddSingleton<AppStateService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
