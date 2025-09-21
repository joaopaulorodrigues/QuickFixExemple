using System.Text.Json;
using OrderGenerator.Application.DependencyInjections;
using OrderGenerator.Infra.DependencyInjections;
using OrderGenerator.Infra.Interfaces;
using OrderGenerator.Infra.Services;
using QuickFix;
using QuickFix.Logger;
using QuickFix.Store;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddApplicationDepedencies();
builder.Services.AddInfraDepedencies();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();