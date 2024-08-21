using CrudOperationsInNetCore.Models;
using CrudOperationsInNetCore.Services;
using Microsoft.AspNetCore.Mvc;

namespace CrudOperationsInNetCore.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDBContext context;
        private readonly IWebHostEnvironment environment;

       

        public ProductController(ApplicationDBContext context, IWebHostEnvironment environment)
        {

            this.context = context;
            this.environment = environment;
        }
        public IActionResult Index()
        {
            var products = context.Products.OrderByDescending(p => p.Id).ToList();
            return View(products);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(ProductDto productDto)
        {
            if(productDto.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "the image file is required");
            }
            if (!ModelState.IsValid)
            {
                return View(productDto);
            }
            //save the image file
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(productDto.ImageFile!.FileName);

            string imageFullPath = environment.WebRootPath + "/Product/" + newFileName;
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                productDto.ImageFile.CopyTo(stream);
            }
            //save the new product in the database
            Product product = new Product()
            {
                Name= productDto.Name,
                    Brand = productDto.Brand,
                    Category = productDto.Category,
                    Price = productDto.Price,
                    Description = productDto.Description,
                    ImageFileName = newFileName,
                    CreatedAt = DateTime.Now,



            };
            context.Products.Add(product);
            context.SaveChanges();

            return RedirectToAction("Index","Products");
        }
    }
}
