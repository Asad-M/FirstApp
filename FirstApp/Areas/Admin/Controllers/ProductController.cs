using FirstApp.DataAccessLayer.Infrastructure.IRepository;
using FirstApp.DataAccessLayer;
using FirstApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FirstApp.Controllers
{
    public class ProductController : Controller
    {
        private IUnitOfWork _unitofwork;
        private IWebHostEnvironment _webHostEnvironment;


        public ProductController(IUnitOfWork unitofwork, IWebHostEnvironment webHostEnvironment)
        {
            _unitofwork = unitofwork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            ProductVM ProductModel = new ProductVM();

            ProductModel.products = _unitofwork.Product.GetAll();//_Context.Categories;
            return View(ProductModel);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Create(Category category)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _unitofwork.Category.Add(category); //_Context.Categories.Add(category);
        //        _unitofwork.Save();//_Context.SaveChanges();
        //        TempData["success"] = "Category Created Done!";
        //        return RedirectToAction("Index");
        //    }
        //    return View();
        //}

        [HttpGet]
        public IActionResult CreateUpdate(int? ID)
        {
            ProductVM ProductModel = new ProductVM()
            {
                CategoriesList = _unitofwork.Category.GetAll().Select(x =>
                new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            if (ID == 0 || ID == null)
            {
                return View(ProductModel);
            }
            else
            {
                ProductModel.product = _unitofwork.Product.GetT(x => x.ID == ID);//_Context.Categories.Find(ID);

                if (ID > 0 && ID != null)
                {
                    return View(ProductModel);
                }
                else
                {
                    return NotFound();
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUpdate(ProductVM Model, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string fileName = string.Empty;
                if(file!=null)
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "ProductImage");
                    fileName=Guid.NewGuid().ToString()+"_"+file.Name;
                    string filePath = Path.Combine(uploadDir, fileName);
                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fs);
                    }
                    Model.product.ImageURL= filePath;
                }

                if(Model.product.ID==0)
                {
                    _unitofwork.Product.Add(Model.product); //_Context.Categories.Add(category);
                    TempData["success"] = "Product Created Done!";
                }
                else
                {
                    _unitofwork.Product.Update(Model.product);//_Context.Categories.Update(category);
                    TempData["success"] = "Product Update Done!";
                }

                _unitofwork.Save();//_Context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Delete(int ID)
        {
            if (ID == 0)
            {
                return NotFound();
            }

            var Product = _unitofwork.Product.GetT(x => x.ID == ID);//_Context.Categories.Find(ID);
            if (Product == null)
            {
                return NotFound();
            }
            return View(Product);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteData(int id)
        {
            var Product = _unitofwork.Product.GetT(x => x.ID == id);//_Context.Categories.Find(id);
            if (Product == null)
            {
                return NotFound();
            }
            _unitofwork.Product.Delete(Product);//_Context.Categories.Remove(cat);
            _unitofwork.Save();//_Context.SaveChanges(true);
            TempData["success"] = "Product Deleted Done!";
            return RedirectToAction("Index");
        }
    }
}
