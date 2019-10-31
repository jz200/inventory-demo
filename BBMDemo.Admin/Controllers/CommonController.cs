using System.Linq;
using Microsoft.AspNetCore.Mvc;
using BBMDemo.Data.Services;
using BBMDemo.Data.Data.Entities;

namespace BBMDemo.Admin.Controllers
{
    [Route("[controller]/[action]")]
    public class CommonController : Controller
    {
        private readonly IDbReadService _dbReadService;
        public CommonController(IDbReadService dbReadService)
        {
            _dbReadService = dbReadService;
        }

        [HttpPost, HttpGet]
        public IActionResult IsItemNumberUnique()
        {
            string itemNumber = Request.QueryString.Value.Split('=')[1];
            var result = _dbReadService.Get<Product>().Any(p => p.ItemNumber.Equals(itemNumber));
            return Json(!result);
        }

        [HttpGet, HttpPost]
        public JsonResult Products(int catId)
        {
            var products = _dbReadService.Get<Product>();
            if (catId > 0)
            {
                products = products.Where(p => p.CategoryId == catId);
            }
            return Json(products.Select(p => new { p.ProductId, p.ItemNumber }));
        }
    }
}