
using Dapr.Client;
using Masa.BuildingBlocks.Service.Caller;

using MASA.IoT.Common.Helper;
using MASA.IoT.Core.Contract.Device;
using Masa.Utils.Models;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .Build();
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMasaBlazor();
builder.Services.AddSingleton(new AppHelper(configuration));
builder.Services.AddAutoRegistrationCaller(typeof(Program).Assembly);

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDaprStarter();
}


var app = builder.Build();
var client = new DaprClientBuilder().Build();
//app.MapPost("/api/device", async () =>
//{
//    client.InvokeMethodAsync<PaginatedListBase<DeviceListViewModel>>(HttpMethod.Post, "masa-iot-service-webapi", "/api/device/DeviceList");
//});
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
