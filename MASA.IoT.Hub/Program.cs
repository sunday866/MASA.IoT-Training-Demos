using MASA.IoT.Hub;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDaprStarter(builder.Configuration.GetSection("DaprOptions"),false);

}

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<HubAppSettings>(builder.Configuration)
    .Configure<HubAppSettings>(settings => settings.EnvironmentName = builder.Environment.EnvironmentName);
builder.Services.AddHostedService<MQHostedService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
