using Api;
using Api.Filters;
using Application.Interfaces.External;
using Application.Interfaces.Processors;
using Application.Interfaces.Rules;
using Application.Interfaces.Services;
using Application.Processors;
using Application.Rules;
using FunBooksAndVideos.PurchaseOrderProcessor.Application.Services;
using Infrastructure.Consumers;
using Infrastructure.Services;
using Infrastructure.Services.External;
using MassTransit;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// --- Configure Services ---
builder.Services.AddControllers();

// Add API Explorer and Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.ExampleFilters();
});

builder.Services.AddSwaggerExamplesFromAssemblyOf<PurchaseOrderExample>();

// MassTransit Configuration
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<ProcessPurchaseOrderConsumer>();
    x.UsingInMemory((context, cfg) => cfg.ConfigureEndpoints(context));
});

// Register services
builder.Services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
builder.Services.AddScoped<IRuleProcessor, RuleProcessor>();
builder.Services.AddScoped<IRule, MembershipActivationRuleHandler>();
builder.Services.AddScoped<IRule, ShippingSlipRuleHandler>();
builder.Services.AddScoped<IMembershipService, MembershipService>();
builder.Services.AddScoped<IShippingSlipService, ShippingSlipService>();
builder.Services.AddScoped<IPurchaseOrderProcessor, PurchaseOrderProcessor>();


// --- Build the Application ---
var app = builder.Build();

// --- Configure Middleware Pipeline ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Purchase Orders API v1");
        // This makes Swagger UI load at the root URL
        c.RoutePrefix = "swagger";
    });
}

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();