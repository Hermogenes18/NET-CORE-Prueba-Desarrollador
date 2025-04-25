using ApiRestTienda.Domain.Entities;
using ApiRestTienda.Infrastructure.Data;
using ApiRestTienda.Middleware;
using ApiRestTienda.Repositories;
using ApiRestTienda.Services;
using ApiRestTienda.Validations;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<ClienteValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSingleton(new DatabaseConnection(connectionString));

builder.Services.AddScoped<IRepository<Cliente>, ClienteRepository>();
builder.Services.AddScoped<IRepository<Producto>, ProductoRepository>();
builder.Services.AddScoped<IRepository<Pedido>, PedidoRepository>();
builder.Services.AddScoped<ProductoRepository>();

builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddScoped<IChatbotService, ChatbotService>();

builder.Services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = "wwwroot";
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseSpa(spa =>
{
    spa.Options.SourcePath = "wwwroot";
});

app.MapFallbackToFile("chatbot.html");

app.UseErrorHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();