using CosmoColonizerAPI.Data;
using CosmoColonizerAPI.Services.Planets;
using CosmoColonizerAPI.Services.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Db")));

builder.Services.AddScoped<IPlanetsService, PlanetsService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

var rsa = RSA.Create();
rsa.ImportSubjectPublicKeyInfo(
    Convert.FromBase64String("MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAsb0aBuJOsN3OGJBWdsIDdYsGL5DABAWYR1+tnUDaHm3yZA3bTXDa9jtEx+yHoHjZ+ivNj/KoQL8so5D7AehnyA5X4IAH84LX9ihdI0+7nrVTkLY32zvsqkjbbqtY7Wapq5pVxo7zjfMkVfTc91WSE0VAj1RUMfZvXAvN2PS+9dQsTncyiK149aZst2o9SO4M4J22yLmezeJkZD/oz68Mcx04pvlSVAgcrs0eXY6ObsrVm5T7bQ91rxuriKosyDnGs76xSN9WbFxWpo2aS28/7csvYK2z0Es9X1a/4tJi3RzIfv0fvPYoFllOtD28GUE6ugJnmQxToaTYb6luR9PJfQIDAQAB"),
    out _
);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "",

            ValidateAudience = false,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new RsaSecurityKey(rsa),
            ValidateLifetime = true
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
