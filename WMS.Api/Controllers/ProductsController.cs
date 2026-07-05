using Microsoft.AspNetCore.Mvc;
using WMS.Application;
using WMS.Domain;

namespace WMS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateProductRequest createProductRequest)
        {
            if(string.IsNullOrWhiteSpace(createProductRequest.Sku) || string.IsNullOrWhiteSpace(createProductRequest.Name))
            {
                return BadRequest("SKU and Name is requested");
            }

            await _productService.CreateProductAsync(createProductRequest.Sku, createProductRequest.Name, createProductRequest.Description);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

           await _productService.DeleteProductAsync(product);

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var products = await _productService.GetAllProductsAsync();

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductByIdAsync(Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductRequest(UpdateProductRequest updateProductRequest, Guid id)
        {
            if (string.IsNullOrWhiteSpace(updateProductRequest.Name))
            {
                return BadRequest("Name is requested");
            }

            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            await _productService.UpdateDetailsAsync(product, updateProductRequest.Name, updateProductRequest.Description);

            return NoContent();
        }
    }
}