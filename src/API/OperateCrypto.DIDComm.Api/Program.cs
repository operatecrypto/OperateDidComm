using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OperateCrypto.DIDComm.Core.Services;
using OperateCrypto.DIDComm.Crypto.Services;
using OperateCrypto.DIDComm.Data.Context;
using OperateCrypto.DIDComm.Data.Repositories;
using OperateCrypto.DIDComm.Handlers;
using OperateCrypto.DIDComm.Handlers.Handlers;
using OperateCrypto.DIDComm.Resolver.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Configure Database (optional - comment out if SQL Server not available)
// var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// builder.Services.AddDbContext<DIDCommDbContext>(options =>
//     options.UseSqlServer(connectionString));

// Configure DIDComm Services
builder.Services.AddScoped<IDIDCommService, DIDCommService>();
builder.Services.AddScoped<IDIDResolver, AccumulateResolver>();
builder.Services.AddScoped<ICryptoService, CryptoService>();

// Configure Message Handlers
builder.Services.AddScoped<IMessageHandler, BasicMessageHandler>();
builder.Services.AddScoped<IMessageHandler, TrustPingHandler>();

// Configure Repositories (using in-memory for testing without database)
builder.Services.AddSingleton<IMessageRepository, InMemoryMessageRepository>();
// builder.Services.AddScoped<IConnectionRepository, ConnectionRepository>();
// builder.Services.AddScoped<IDIDCommKeyRepository, DIDCommKeyRepository>();
// builder.Services.AddScoped<IMessageThreadRepository, MessageThreadRepository>();

// Configure HttpClient for DID resolution
builder.Services.AddHttpClient<IDIDResolver, AccumulateResolver>();

// Configure Authentication (JWT)
var jwtSettings = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Key is required");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.AddAuthorization();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "OperateCrypto DIDComm API",
        Version = "v1",
        Description = "DIDComm v2 Messaging Infrastructure for OperateCrypto Web3 Wallet",
        Contact = new OpenApiContact
        {
            Name = "OperateCrypto",
            Url = new Uri("https://operatecrypto.com")
        }
    });

    // Add JWT authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
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

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "DIDComm API V1");
        c.RoutePrefix = string.Empty; // Serve Swagger UI at root
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
