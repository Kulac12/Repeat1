using Business.Abstract;
using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")] //ben bu isteği yaptığımda bu insanlar bana nasıl ulaşsın ?? /api/Products ile ulaşabilirsin demek.
    [ApiController]
    public class ProductsController : ControllerBase
    {

        //Loosely copied
        //naming convention
        IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;




        }

        [HttpGet("merhaba")]

        public string Get()
        {
            return "Merhaba";
        }

        [HttpGet("listProducts")]
        public List<Product> GetProducts()
        {
            return new List<Product>
            {
                new Product{ ProductId=1, ProductName="Bere"},
                new Product{ ProductId=2, ProductName="Elma"},
                new Product{ ProductId=3, ProductName="Radyo"},
                new Product{ ProductId=4, ProductName="Battaniye"},
                new Product{ ProductId=5, ProductName="Anahtar"},
                new Product{ ProductId=6, ProductName="Takı"},

            };
        }

        [HttpGet("deneme")]
        public List<Product> GetDeneme()
        {
            IProductService productService = new ProductManager(new EfProductDal());
            var result = productService.GetAll();
            return result.Data;
        }

        [HttpGet("deneme2")]
        public string GetDeneme2()
        {
            var result = _productService.GetAll();
            return result.Message;

        }


     
    }
}
