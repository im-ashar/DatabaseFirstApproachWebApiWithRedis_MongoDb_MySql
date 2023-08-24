using Dapper;
using DatabaseFirstApproachWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatabaseFirstApproachWebApi.Controllers
{
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class EcommerceController : ControllerBase
    {
        readonly EcommerceContext _context;
        public EcommerceController(EcommerceContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product, string companyName, string categoryName)
        {
            using var con = _context.Database.GetDbConnection();

            string query = "SELECT company.id FROM company WHERE company.name=@Name";
            var companyId = await con.ExecuteScalarAsync<int>(query, new { Name = companyName });

            query = "SELECT category.id FROM category WHERE category.name=@Name";
            var categoryId = await con.ExecuteScalarAsync<int>(query, new { Name = categoryName });


            query = "INSERT INTO Product (Name,Company_Id,Category_Id) VALUES (@Name,@CompanyName,@CategoryName)";
            await con.ExecuteAsync(query, new { Name = product.Name, CompanyName = companyId, CategoryName = categoryId });
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(Category category)
        {
            using var con = _context.Database.GetDbConnection();
            string query = "INSERT INTO Category (Name) VALUES (@Name)";
            await con.ExecuteAsync(query, category);
            return Ok(category);
        }
        [HttpPost]
        public async Task<IActionResult> AddCompany(Company company)
        {
            using var con = _context.Database.GetDbConnection();
            string query = "INSERT INTO Company (Name) VALUES (@Name)";
            await con.ExecuteAsync(query, company);
            return Ok(company);
        }
        [HttpPost]
        public async Task<IActionResult> AddCategoryToCompany(string category, string company)
        {
            using var con = _context.Database.GetDbConnection();
            string query = "SELECT company.id FROM company WHERE company.name=@Name";
            var companyId = await con.ExecuteScalarAsync<int>(query, new { Name = company });

            query = "SELECT category.id FROM category WHERE category.name=@Name";
            var categoryId = await con.ExecuteScalarAsync<int>(query, new { Name = category });

            query = "INSERT INTO company_category (company_id, category_id) VALUES (@companyId,@categoryId)";
            await con.ExecuteAsync(query, new { companyId = companyId, categoryId = categoryId });
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            using var con = _context.Database.GetDbConnection();
            string query = "SELECT * FROM category";
            var categories = (await con.QueryAsync<Category>(query)).ToList();
            return Ok(categories);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCompanies()
        {
            using var con = _context.Database.GetDbConnection();
            string query = "SELECT * FROM company";
            var companies = (await con.QueryAsync<Company>(query)).ToList();
            return Ok(companies);
        }
        [HttpGet]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            using var con = _context.Database.GetDbConnection();
            string query = "SELECT * FROM category WHERE category.id=@id";
            var category = await con.QueryAsync<Category>(query, new { id = id });
            return Ok(category);
        }
        [HttpGet]
        public async Task<IActionResult> GetCompanyById(int id)
        {
            using var con = _context.Database.GetDbConnection();
            string query = "SELECT * FROM company WHERE company.id=@id";
            var company = await con.QueryAsync<Company>(query, new { id = id });
            return Ok(company);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            using var con = _context.Database.GetDbConnection();
            string query = "SELECT product.id,company.NAME,category.NAME,product.NAME FROM product JOIN company ON product.company_id = company.id JOIN category ON product.category_id = category.id";
            var products = (await con.QueryAsync<Product>(query)).ToList();
            return Ok(products);
        }
        [HttpPost]
        public async Task<IActionResult> GetProductById(int id)
        {
            using var con = _context.Database.GetDbConnection();
            string query = "SELECT product.id,company.NAME,category.NAME,product.NAME FROM product JOIN company ON product.company_id = company.id JOIN category ON product.category_id = category.id WHERE product.id=@id";
            var product = await con.QueryAsync<Product>(query, new { id = id });
            return Ok(product);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteProductById(int id)
        {
            using var con = _context.Database.GetDbConnection();
            string query = "DELETE FROM product WHERE product.id=@id";
            await con.ExecuteAsync(query, new { id = id });
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteCategoryById(int id)
        {
            using var con = _context.Database.GetDbConnection();
            string query = "DELETE FROM category WHERE category.id=@id";
            await con.ExecuteAsync(query, new { id = id });
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteCompanyById(int id)
        {
            using var con = _context.Database.GetDbConnection();
            string query = "DELETE FROM company WHERE company.id=@id";
            await con.ExecuteAsync(query, new { id = id });
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> UpdateProduct(Product product)
        {
            using var con = _context.Database.GetDbConnection();
            string query = "SELECT * FROM product WHERE product.id = @Id";
            var pro = await con.QueryFirstOrDefaultAsync<Product>(query, new { id = product.Id });
            if (pro != null)
            {
                query = "UPDATE product SET Name = @Name WHERE Id = @id";
                await con.ExecuteAsync(query, new { Name = product.Name, id = product.Id });
                return Ok(product);
            }
            var err = new { Message = "Product Not Found" };
            return StatusCode(StatusCodes.Status404NotFound, err);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCategory(Category category)
        {
            using var con = _context.Database.GetDbConnection();
            string query = "SELECT * FROM category WHERE category.id = @Id";
            var cat = await con.QueryFirstOrDefaultAsync<Category>(query, new { id = category.Id });
            if (cat != null)
            {
                query = "UPDATE category SET Name = @Name WHERE Id = @id";
                await con.ExecuteAsync(query, new { Name = category.Name, id = category.Id });
                return Ok(category);
            }
            var err = new { Message = "Category Not Found" };
            return StatusCode(StatusCodes.Status404NotFound, err);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCompany(Company company)
        {
            using var con = _context.Database.GetDbConnection();
            string query = "SELECT * FROM company WHERE company.id = @Id";
            var com = await con.QueryFirstOrDefaultAsync<Company>(query, new { id = company.Id });
            if (com != null)
            {
                query = "UPDATE company SET Name = @Name WHERE Id = @id";
                await con.ExecuteAsync(query, new { Name=company.Name,id = company.Id });
                return Ok(company);
            }
            var err = new { Message = "Company Not Found" };
            return StatusCode(StatusCodes.Status404NotFound, err);
        }
    }
}
