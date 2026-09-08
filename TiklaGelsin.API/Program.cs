using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TiklaGelsin.Application.Factories;
using TiklaGelsin.Application.Interfaces;
using TiklaGelsin.Application.Strategies;
using TiklaGelsin.Domain.Interfaces;
using TiklaGelsin.Infrastructure.ExternalServices;
using TiklaGelsin.Infrastructure.Persistence;

using TiklaGelsin.Infrastructure.Services;
using TiklaGelsin.Infrastructure.Repositories;
using TiklaGelsin.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// 1. DbContext Configuration (EF Core with PostgreSQL)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Dependency Injection for Clean Architecture
builder.Services.AddScoped<IExternalPaymentService, IyzicoPaymentClient>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderQueryService, OrderQueryService>();
builder.Services.AddScoped<CheckoutService>();

// Register AutoMapper
builder.Services.AddAutoMapper(cfg => 
{
    cfg.AddProfile<TiklaGelsin.Application.Mapping.ApplicationMappingProfile>();
    cfg.AddProfile<TiklaGelsin.Infrastructure.Mapping.InfrastructureMappingProfile>();
});

// Register Payment Strategies
builder.Services.AddScoped<IPaymentMethod, CreditCardPayment>();
builder.Services.AddScoped<IPaymentMethod, MealCardPayment>();
builder.Services.AddScoped<IPaymentMethod, CashOnDeliveryPayment>();

// Register Factory
builder.Services.AddScoped<IPaymentFactory, PaymentFactory>();

// 3. JWT Authentication Configuration
var key = Encoding.ASCII.GetBytes("SuperSecretKeyForJwtAuthentication12345!!");
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
