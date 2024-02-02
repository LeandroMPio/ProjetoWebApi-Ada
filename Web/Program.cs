
using Application.Services;
using Domain.Entities;
using Domain.Options;
using Domain.Requests;
using Domain.Validators;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Web.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<TokenOptions>(
    builder.Configuration.GetSection(TokenOptions.Section));

builder.Services.Configure<PasswordHashOptions>(
    builder.Configuration.GetSection(PasswordHashOptions.Section));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<Context>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlDatabase"));
});

builder.Services.AddScoped<IHashingService, HashingService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();


builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IValidator<BaseUserRequest>, UserValidator>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IValidator<BaseProductRequest>, ProductValidator>();

var app = builder.Build();

// Middlewares

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var services = builder.Services.BuildServiceProvider();
var context = services.GetRequiredService<Context>();

context.Database.EnsureCreatedAsync().Wait();

app.Run();
