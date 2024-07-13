using System.Text;
using backend;
using backend.Data;
using backend.Entity;
using backend.Mapper;
using backend.Repositories;
using backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "LocalBrand API", Version = "v1" });

    // Configure Swagger to use the JWT token
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your token in the text input below.\n\nExample: \"Bearer ey8asd....\""
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
            new string[] {}
        }
    });
});


//AutoMapper
builder.Services.AddAutoMapper(typeof(MapperConfig));

//DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// JWT Configuration
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]);
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
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});


//Service
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<ICartService, CartService>();

//Repository
builder.Services.AddScoped(typeof(Repository<>), typeof(Repository<>));
builder.Services.AddScoped<IRepository<Review>, Repository<Review>>();
builder.Services.AddScoped<IRepository<Category>, Repository<Category>>();
builder.Services.AddScoped<IRepository<Size>, Repository<Size>>();
builder.Services.AddScoped<IRepository<ProductInventory>, Repository<ProductInventory>>();
builder.Services.AddScoped<IRepository<ProductImage>, Repository<ProductImage>>();
builder.Services.AddScoped<IRepository<Cart>, Repository<Cart>>();
builder.Services.AddScoped<IRepository<CartItem>, Repository<CartItem>>();
builder.Services.AddScoped<IRepository<User>, Repository<User>>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();

builder.Services.AddAuthorization();

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

