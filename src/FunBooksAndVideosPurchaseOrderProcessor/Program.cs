using FunBooksAndVideos.PurchaseOrderProcessor.Application.Interfaces;
using FunBooksAndVideos.PurchaseOrderProcessor.Application.Services;
using FunBooksAndVideos.PurchaseOrderProcessor.Infrastructure.Consumers;
using FunBooksAndVideos.PurchaseOrderProcessor.Infrastructure.Repositories;
using FunBooksAndVideos.PurchaseOrderProcessor.Persistence;
using FunBooksAndVideos.PurchaseOrderProcessor.Shared.Mapping;
using FunBooksAndVideos.PurchaseOrderProcessor.Shared.Validation;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// --- Configure Services (Dependency Injection) ---

// Add Controllers
builder.Services.AddControllers();

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add MediatR - Register handlers from the Application assembly
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.Load("FunBooksAndVideos.PurchaseOrderProcessor.Application")));

// Add MassTransit with In-Memory Transport
builder.Services.AddMassTransit(x =>
{
    // Register Consumers from the Infrastructure assembly
    x.AddConsumer<GenerateShippingSlipConsumer>();
    x.AddConsumer<ActivateMembershipConsumer>();

    x.UsingInMemory((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });
});
// Optional: If using older MassTransit version, might need AddMassTransitHostedService()
// builder.Services.AddMassTransitHostedService(); // For MassTransit v7 and below

// Add EF Core DbContext with SQLite
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=FunBooksAndVideos.db";
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

// Register Application Layer Services & Interfaces
builder.Services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();

// Register Shared Kernel Components (Validators, Mappers)
// Assuming these are stateless, Singleton or Scoped is fine. Scoped is safer.
builder.Services.AddScoped<IPurchaseOrderValidator, PurchaseOrderValidator>();
builder.Services.AddScoped<IPurchaseOrderMapper, PurchaseOrderMapper>();

// Register Infrastructure Layer Services (Repositories)
builder.Services.AddScoped<IPurchaseOrderRepository, SqlitePurchaseOrderRepository>();

// --- Build the Application ---
var app = builder.Build();

// --- Configure the HTTP request pipeline ---

// Use Swagger in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Add authorization if needed
// app.UseAuthorization();

app.MapControllers();

// --- Run the Application ---
app.Run();

