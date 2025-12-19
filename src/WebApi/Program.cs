using DanCart.DataAccess.Blob;
using DanCart.DataAccess.Data;
using DanCart.DataAccess.DBInitializer;
using DanCart.DataAccess.Models;
using DanCart.DataAccess.Repository;
using DanCart.DataAccess.Repository.IRepository;
using DanCart.Models.Auth;
using DanCart.WebApi;
using DanCart.WebApi.Areas.Auth.Services;
using DanCart.WebApi.Areas.Auth.Services.IServices;
using DanCart.WebApi.Areas.Checkouts.Services;
using DanCart.WebApi.Areas.Checkouts.Services.IServices;
using DanCart.WebApi.Areas.Customers.Services;
using DanCart.WebApi.Areas.Customers.Services.IServices;
using DanCart.WebApi.Areas.Products.Services;
using DanCart.WebApi.Areas.Products.Services.IServices;
using DanCart.WebApi.Areas.SalesOrders.Services;
using DanCart.WebApi.Areas.SalesOrders.Services.IServices;
using DanCart.WebApi.Areas.ShoppingCarts.Services;
using DanCart.WebApi.Areas.ShoppingCarts.Services.IServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Stripe;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ClockSkew = TimeSpan.FromSeconds(30),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Key").Get<string>()))
    };
});

builder.Services.AddScoped<IDBInitializer, DBInitializer>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IBlobService, AzureBlobService>();

builder.Services.AddScoped<ICheckoutService, DanCart.WebApi.Areas.Checkouts.Services.CheckoutService>();
builder.Services.AddScoped<ITokenProviderService, TokenProviderService>();

builder.Services.AddScoped<IProductsBlobService, ProductsBlobService>();

builder.Services.AddScoped<IProductsService, ProductsService>();
builder.Services.AddScoped<ISalesOrdersService, SalesOrdersService>();
builder.Services.AddScoped<ICustomerService, DanCart.WebApi.Areas.Customers.Services.CustomerService>();

builder.Services.AddScoped<ICustomerMetricsService, CustomerMetricsService>();
builder.Services.AddScoped<IProductMetricsService, ProductMetricsService>();
builder.Services.AddScoped<ISalesOrderMetricsService, SalesOrderMetricsService>();
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();

builder.Services.AddControllers();
builder.Services.AddAutoMapper(cfg => { cfg.AddProfile<MappingProfile>(); });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            }, []
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "DanCart API V1"));
}


app.UseHttpsRedirection();
app.UseCors("AllowAll");
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

SeedDatabase();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

void SeedDatabase()
{
    using var scope = app.Services.CreateScope();
    var dbInitializer = scope.ServiceProvider.GetRequiredService<IDBInitializer>();
    dbInitializer.Initialize();
}

public partial class Program;