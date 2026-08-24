using GiftCardSystem.Api.Helper;
using GiftCardSystem.Database.AppDbContextModels;
using GiftCardSystem.Repository.GiftCards;
using GiftCardSystem.Repository.PaymentMethods;
using GiftCardSystem.Repository.Tickets;
using GiftCardSystem.Repository.Transactions;
using GiftCardSystem.Repository.Users;
using GiftCardSystem.Service.Auth;
using GiftCardSystem.Service.GiftCards;
using GiftCardSystem.Service.PaymentMethods;
using GiftCardSystem.Service.Purchase;
using GiftCardSystem.Service.Store;
using GiftCardSystem.Service.Tickets;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "GiftCardSystem API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT access token below (no need to type 'Bearer')."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();
builder.Services.AddScoped<IGiftCardRepository, GiftCardRepository>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<PaymentMethodService>();
builder.Services.AddScoped<GiftCardService>();
builder.Services.AddScoped<TicketService>();
builder.Services.AddScoped<StoreService>();
builder.Services.AddScoped<PurchaseService>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<HttpContextService>();

var jwt = builder.Configuration.GetSection("JwtSetting");
var keyBytes = Encoding.UTF8.GetBytes(jwt["Key"]!);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,         
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
            ClockSkew = TimeSpan.Zero          
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "GiftCardSystem API v1");
    });
}

app.UseHttpsRedirection();

app.UseStaticFiles();   // serves the generated QR images from wwwroot/qrcodes

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
