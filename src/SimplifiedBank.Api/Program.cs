using SimplifiedBank.Api.Configuration;
using SimplifiedBank.Application.Services;
using SimplifiedBank.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddApplicationConfigurations();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApiConfigurations();
builder.Services.AddDocumentation();

var app = builder.Build();

app.ConfigureApp();

app.Run();

