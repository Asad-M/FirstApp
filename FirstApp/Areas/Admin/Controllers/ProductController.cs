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
        #region APICAll
        public IActionResult AllProducts()
        {
            var products = _unitofwork.Product.GetAll(includePropties: "Category");
            return Json(new { data = products });
        }
        #endregion
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
                if (file != null)
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "ProductImage");
                    string getExtention=Path.GetExtension(file.FileName);
                    fileName = Guid.NewGuid().ToString() + "_" + file.Name+ getExtention;
                    string filePath = Path.Combine(uploadDir, fileName);
                    if (Model.product.ImageURL != null)
                    {
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, Model.product.ImageURL.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fs);
                    }
                    Model.product.ImageURL = filePath;
                }

                if (Model.product.ID == 0)
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

        #region DeleteAPICALL

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var Product = _unitofwork.Product.GetT(x => x.ID == id);//_Context.Categories.Find(id);
            if (Product == null)
            {
                return Json(new { success = false, message = "Error in Fetching Data" });
            }
            else
            {
                var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, Product.ImageURL.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }

                _unitofwork.Product.Delete(Product);//_Context.Categories.Remove(cat);
                _unitofwork.Save();//_Context.SaveChanges(true);
                return Json(new { success = true, message = "Product Deleted" });

            }
            
        }
        #endregion
    }
}
