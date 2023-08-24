using DatabaseFirstApproachWebApi.Configurations;
using DatabaseFirstApproachWebApi.Models;
using DatabaseFirstApproachWebApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

string connString = builder.Configuration.GetConnectionString("MySqlConnString")!;
// Add services to the container.
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("MongoDatabase"));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContextPool<EcommerceContext>(options => options.UseMySql(connString, ServerVersion.AutoDetect(connString)));
builder.Services.AddScoped<IProductService, ProductsService>();
builder.Services.AddScoped<IRedisService, RedisService>();
builder.Services.AddStackExchangeRedisCache(options =>
{
	options.Configuration = builder.Configuration.GetConnectionString("Redis");
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
