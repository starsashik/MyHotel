using System.Text;
using hotel_backend.Abstractions.Repositories;
using hotel_backend.Abstractions.Services;
using hotel_backend.DataBase.Contexts;
using hotel_backend.DataBase.Repositories;
using hotel_backend.Exceptions.SpecificExceptions;
using hotel_backend.Middlewares;
using hotel_backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(
    new WebApplicationOptions {WebRootPath = "Static"}); // изменяем папку для хранения статики

//builder.Services.AddControllers();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // Сохранять имена свойств как есть
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Authentication and authorization configuration
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = true;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],

            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],

            ValidateLifetime = true,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]
                                       ?? throw new ConfigurationException("JWT-key is missing")))
        };
    });
builder.Services.AddAuthorization();

// Services for access rights demarcation
builder.Services.AddScoped<IAccessCheckService, AccessCheckService>();
builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();

// Services for services
builder.Services.AddScoped<IRolesService, RolesService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IHotelsService, HotelsService>();
builder.Services.AddScoped<IRoomsService, RoomsService>();
builder.Services.AddScoped<IBookingService, BookingService>();

builder.Services.AddScoped<ILogService, SimpleLogService>();

// Services for repositories
builder.Services.AddScoped<IRolesRepository, RolesRepository>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IHotelsRepository, HotelsRepository>();
builder.Services.AddScoped<IRoomsRepository, RoomsRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();

// Database context;
builder.Services.AddDbContext<MyDbContext>(options  =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("MyDbContext")));

// CORS settings
builder.Services.AddCors(options => options.AddPolicy
    (
        "MyFirstapp", b => b
            // .WithOrigins(builder.Configuration["Frontend:FrontendAddress"]
            //              ?? throw new ConfigurationException("Frontend address is missing"))// Принимается запрос только с определенных адресов
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()// Принимаются запросы с любыми заголовками
            .AllowAnyMethod()// Принимаются запросы с любого типа (GET/POST)
            .AllowCredentials()// Разрешается принимать идентификационные данные от клиента (например, куки)
    )
);

// Добавляем поддержку сессий
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Error handling middleware
app.UseMiddleware<ErrorHandlingMiddleware>();

// Authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Docker"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("MyFirstapp");

// Поддержка статических файлов
app.UseStaticFiles();
app.UseFileServer();

// Поддержка сессии
app.UseSession();

app.UseRouting();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();