using DatabaseFirstApproachWebApi.Models;

namespace DatabaseFirstApproachWebApi.Services
{
	public interface IProductService
	{
		Task<List<ProductForMongo>> GetAllProductsAsync();
		Task CreateProductAsync(ProductForMongo product);
		Task DeleteProductAsync(string id);
		Task<ProductForMongo> GetProductByIdAsync(string id);


	}
}
