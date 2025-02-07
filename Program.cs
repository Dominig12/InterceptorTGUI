using System.Diagnostics;
using System.Text.RegularExpressions;
using InterceptorTGUI;
using InterceptorTGUI.Service;
using Microsoft.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(new DataProvider());

RefreshConfigXml();
FileInjector fileInjector = new FileInjector("var message = Byond.parseJson(rawMessage);",
    $"var message = Byond.parseJson(rawMessage);\n      fetch(\"http://localhost:5000/api/data\", {{\n        method: 'POST',\n        headers: {{ 'Content-Type': 'application/json' }},\n        body: JSON.stringify(message)\n      }});",
    "tgui-window-*.html");
fileInjector.Start();

// FileInjector fileInjector2 = new FileInjector("NanoStateManager.receiveUpdateData(jsonString);",
//     "NanoStateManager.receiveUpdateData(jsonString);\n\t\t\t\t\tfetch(\"http://localhost:5000/api/data\", {\n\t\t\t\t\t\t method: 'POST',       \n\t\t\t\t\t\t headers: { 'Content-Type': 'application/json' },       \n\t\t\t\t\t\t body: jsonString      \n\t\t\t\t\t});",
//     "*.htm");
// fileInjector2.Start();


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
