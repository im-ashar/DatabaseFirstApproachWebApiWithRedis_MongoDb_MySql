using DatabaseFirstApproachWebApi.Models;
using DatabaseFirstApproachWebApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace DatabaseFirstApproachWebApi.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class MongoDbController : ControllerBase
	{
		private readonly IProductService _productService;
		private readonly IRedisService _redisService;

		public MongoDbController(IProductService productService, IRedisService redisService)
		{
			_productService = productService;
			_redisService = redisService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllProducts()
		{
			var cacheData = await _redisService.GetAsync("Products");
			if (cacheData != null)
			{
				Console.WriteLine("From Cache");
				var result = JsonSerializer.Deserialize<List<ProductForMongo>>(cacheData);
				return Ok(result);
			}
			var getFromDb = await _productService.GetAllProductsAsync();
			await _redisService.AddAsync("Products", JsonSerializer.Serialize(getFromDb));
			return Ok(getFromDb);
		}
		[HttpPost]
		public async Task<IActionResult> CreateProduct(ProductForMongo product)
		{
			await _productService.CreateProductAsync(product);
			await _redisService.AddAsync($"Product-{product.Id}", JsonSerializer.Serialize(product));
			var upatedList = await _productService.GetAllProductsAsync();
			await _redisService.AddAsync("Products", JsonSerializer.Serialize(upatedList));
			return Ok(product);
		}
		[HttpPost]
		public async Task<IActionResult> DeleteProduct(string id)
		{
			var result = await _productService.GetProductByIdAsync(id);
			if (result != null)
			{
				Console.WriteLine("From Cache");

				await _redisService.DeleteAsync($"Product-{id}");
				await _productService.DeleteProductAsync(id);
				var upatedList = await _productService.GetAllProductsAsync();
				await _redisService.AddAsync("Products", JsonSerializer.Serialize(upatedList));
				return Ok();
			}
			var err = new { message = "Product not found" };
			return StatusCode(StatusCodes.Status404NotFound, err);
		}
		[HttpGet]
		public async Task<IActionResult> GetProductById(string id)
		{
			var result = await _redisService.GetAsync($"Product-{id}");
			if (result != null)
			{
				Console.WriteLine("From Cache");
				return Ok(result);
			}
			var product = await _productService.GetProductByIdAsync(id);
			if (product != null)
			{
				await _redisService.AddAsync($"Product-{product.Id}", JsonSerializer.Serialize(product));
				return Ok(product);
			}
			var err = new { message = "Product Not found" };
			return StatusCode(StatusCodes.Status404NotFound, err);
		}
	}
}
