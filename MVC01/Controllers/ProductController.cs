using Microsoft.AspNetCore.Mvc;
using MVC01.Models;

namespace MVC01.Controllers
{
    [Route("test/[action]")]
    public class ProductController : Controller
    {

        // 1. Initial Version: Returning ContentResult manually
        public ContentResult Index_Manual()
        {
            ContentResult result = new ContentResult();
            result.Content = "Hello from ProductController";
            return result;
        }

        // 2. First Update: Using the Content() Helper Method
        public ContentResult Index_Helper()
        {
            return Content("Hello from ProductController");
        }

        // 3. Second Update: Returning ViewResult
        public ViewResult Index()
        {
            return View();
        }

        // 4. Returning JsonResult
        public JsonResult GetProducts()
        {
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Laptop", Price = 15000 },
                new Product { Id = 2, Name = "Mouse", Price = 300 }
            };
            return Json(products);
        }

        // 5. Returning IActionResult with multiple conditional outcomes
        public IActionResult GetProductById(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid ID");

            if (id == 100)
                return RedirectToAction("GetProducts");

            var products = new List<Product>
            {
                new Product { Id=15 , Name ="Laptop" },
                new Product { Id=20 , Name ="Mobile" }
            };
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound();

            return Json(product);
        }

        // 6. External Redirect
        public RedirectResult RedirectToLink()
        {
            return Redirect("https://www.google.com");
        }

        // 7. Returning a downloadable file (FileResult)
        public FileResult FileAction()
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes("wwwroot/files/LINQAssignment.pdf");
            return File(fileBytes, "application/pdf", "TestFile.pdf");
        }

        // 8. Attribute Routing
        [HttpGet("json")]
        public JsonResult JsonAction()
        {
            return Json(new { Message = "Attribute Routing Works!" });
        }

        [HttpGet("ProductCode/{code}")]
        public IActionResult GetProductByCode(int code)
        {
            return Content($"Product Code: {code}");
        }

        // 9. Model Binding - Initial Version (Route value takes priority over Query String)
        public IActionResult Search(int id)
        {
            return Content($"Id: {id}");
        }

        // 10. Model Binding - First Update (Forcing binding from Query String)
        public IActionResult SearchFromQuery([FromQuery] int id)
        {
            return Content($"Id from Query: {id}");
        }

        // 11. Model Binding - Receiving multiple separate parameters
        public IActionResult FilterSeparate(string term, decimal minPrice, int page)
        {
            return Content($"Term: {term}, MinPrice: {minPrice}, Page: {page}");
        }

        // 12. Model Binding - Update: Grouping parameters into a Complex Type
        public IActionResult FilterComplex(ProductFilter filter)
        {
            return Content($"Term: {filter.Term}, MinPrice: {filter.MinPrice}, Page: {filter.Page}");
        }

        // 13. Model Binding - Mapping a custom Query String parameter name
        public IActionResult GetById([FromQuery] int id, [FromQuery(Name = "id2")] int otherId)
        {
            return Content($"Id: {id}, OtherId: {otherId}");
        }
    }
}

