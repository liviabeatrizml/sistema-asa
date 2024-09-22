using Front_end;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Net.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configure HttpClient para a base da sua API
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5500/") });
builder.Services.AddScoped<DiscenteService>();

await builder.Build().RunAsync();
