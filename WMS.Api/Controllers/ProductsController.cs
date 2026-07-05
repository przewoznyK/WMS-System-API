using Microsoft.AspNetCore.Mvc;
using WMS.Application;

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
        public IActionResult Create(CreateProductRequest createProductRequest)
        {
            if(string.IsNullOrWhiteSpace(createProductRequest.Sku) || string.IsNullOrWhiteSpace(createProductRequest.Name))
            {
                return BadRequest("SKU and Name is requested");
            }

            _productService.CreateProduct(createProductRequest.Sku, createProductRequest.Name, createProductRequest.Description);

            return Ok();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var products = _productService.GetAllProducts();

            return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult GetProductById(Guid id)
        {
            var product = _productService.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProductRequest(UpdateProductRequest updateProductRequest, Guid id)
        {
            if (string.IsNullOrWhiteSpace(updateProductRequest.Name))
            {
                return BadRequest("Name is requested");
            }

            var product = _productService.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            _productService.UpdateDetails(product, updateProductRequest.Name, updateProductRequest.Description);

            return NoContent();
        }
    }
}