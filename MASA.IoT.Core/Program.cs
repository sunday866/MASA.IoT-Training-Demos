
using MASA.IoT.WebApi;
using MASA.IoT.WebApi.Handler;
using MASA.IoT.WebApi.IHandler;
using MASA.IoT.WebApi.Models.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddDapr();
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDaprStarter();
}
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMapster();
builder.Services.AddTransient<IDeviceHandler, DeviceHandler>();
builder.Services.AddTransient<IMqttHandler, MqttHandler>();
builder.Services.Configure<AppSettings>(builder.Configuration)
    .Configure<AppSettings>(settings => settings.EnvironmentName = builder.Environment.EnvironmentName);

builder.Services.AddDbContext<MASAIoTContext>(
    options => options.UseSqlServer("name=ConnectionStrings:DefaultConnection"));

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();
app.UseCloudEvents();
app.MapControllers();
app.MapSubscribeHandler();


app.Run();
