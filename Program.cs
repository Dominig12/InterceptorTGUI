using System.Diagnostics;
using System.Text.RegularExpressions;
using InterceptorTGUI;
using Microsoft.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
RefreshConfigXml();
FileInjector fileInjector = new FileInjector();
fileInjector.Start();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builderCors =>
    {
        builderCors.AllowAnyOrigin() // Разрешаем доступ с любого адреса
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

WebApplication app = builder.Build();

// Configure the HTTP request pipeline. 

app.MapControllers();
app.UseCors();
app.Run();
return;


void RefreshConfigXml()
{
    if (!System.IO.File.Exists("config.xml"))
    {
        Config.SaveConfig();
    }
    else
    {
        Config.LoadConfig();
    }
}
