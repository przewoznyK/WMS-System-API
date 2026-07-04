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
            _productService.CreateProduct(createProductRequest.Sku, createProductRequest.Name, createProductRequest.Description);

            return Ok();
        }
    }
}