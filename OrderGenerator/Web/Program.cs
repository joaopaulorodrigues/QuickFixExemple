using OrderGenerator.Application.DependencyInjections;
using FluentValidation;
using OrderGenerator.Domain.NewOrder;
using OrderGenerator.Infra.DependencyInjections;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddApplicationDepedencies();
builder.Services.AddInfraDepedencies();
builder.Services.AddScoped<IValidator<NewOrderRequest>, NewOrderRequestValidator>();


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