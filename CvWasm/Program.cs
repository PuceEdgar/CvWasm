using CvWasm;
using CvWasm.Managers;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSingleton<IFileService, FileService>();
builder.Services.AddSingleton<IComponentService, ComponentService>();
builder.Services.AddSingleton<IJsService, JsService>();
builder.Services.AddSingleton<ICommandService, CommandService>();

await builder.Build().RunAsync();
