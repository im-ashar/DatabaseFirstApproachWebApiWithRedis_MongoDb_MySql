using DatabaseFirstApproachWebApi.Configurations;
using DatabaseFirstApproachWebApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DatabaseFirstApproachWebApi.Services
{
	public class ProductsService : IProductService
	{
		private readonly IMongoCollection<ProductForMongo> products;

		public ProductsService(IOptions<DatabaseSettings> databaseSettings)
		{
			var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
			var mongoDb = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
			products = mongoDb.GetCollection<ProductForMongo>(databaseSettings.Value.CollectionName);
		}
		public async Task<List<ProductForMongo>> GetAllProductsAsync()
		{
			return await (await products.FindAsync(x => true)).ToListAsync();
		}
		public async Task CreateProductAsync(ProductForMongo product)
		{
			await products.InsertOneAsync(product);
		}

		public async Task DeleteProductAsync(string id)
		{
			await products.DeleteOneAsync(x => x.Id == id);
		}
		public async Task<ProductForMongo> GetProductByIdAsync(string id)
		{
			var product = await (await products.FindAsync(x => x.Id == id)).FirstOrDefaultAsync();
			if (product != null)
			{
				return product;
			}
			return default;
		}
	}
}
