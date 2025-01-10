using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SajdaUz.Prayer;
using SajdaUz.Prayer.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<PrayerTimeService>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://api.aladhan.com/v1/") });

await builder.Build().RunAsync();
