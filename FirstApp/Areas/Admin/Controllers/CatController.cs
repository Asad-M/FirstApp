using FirstApp.DataAccessLayer.Infrastructure.IRepository;
using FirstApp.DataAccessLayer;
using FirstApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace FirstApp.Controllers
{
    public class CatController : Controller
    {
        private IUnitOfWork _unitofwork;
            

        public CatController(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }

        public IActionResult Index()
        {
            CategoryVM VM=new CategoryVM();

             VM.categories= _unitofwork.Category.GetAll();//_Context.Categories;
            return View(VM);
        }
        //[HttpGet]
        //public IActionResult Create()
        //{
        //    return View();
        //}
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
            CategoryVM category=new CategoryVM();
            if(ID == 0 || ID == null)
            {
                return View(category);
            }
            else
            {
                category.category = _unitofwork.Category.GetT(x => x.Id == ID);//_Context.Categories.Find(ID);
                
                if (ID > 0 && ID!=null)
                {
                    return View(category);
                }
                else
                {
                    return NotFound();
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitofwork.Category.Update(category);//_Context.Categories.Update(category);
                _unitofwork.Save();//_Context.SaveChanges();
                TempData["success"] = "Category Updated Done!";
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

            var category = _unitofwork.Category.GetT(x => x.Id == ID);//_Context.Categories.Find(ID);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteData(int id)
        {
            var cat= _unitofwork.Category.GetT(x => x.Id == id);//_Context.Categories.Find(id);
            if (cat == null)
            {
                return NotFound();
            }
            _unitofwork.Category.Delete(cat);//_Context.Categories.Remove(cat);
            _unitofwork.Save();//_Context.SaveChanges(true);
            TempData["success"] = "Category Deleted Done!";
            return RedirectToAction("Index");
        }
    }
}
