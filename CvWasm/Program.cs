using CvWasm;
using CvWasm.Managers;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSingleton<IFileManager, FileManager>();
//builder.Services.AddSingleton<IErrorManager, ErrorManager>();
builder.Services.AddSingleton<IComponentManager, ComponentManager>();
builder.Services.AddSingleton<IJsService, JsService>();
builder.Services.AddSingleton<ICommandService, CommandService>();
builder.Services.AddSingleton<StateContainer>();

await builder.Build().RunAsync();
