
using Bulky.DataAcess.Repository.IRepsitory;
using BulkyAcess.DataAcess.Data;
using BulkyAcess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using BulkyAcess.Utility;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]

    public class CategoryController : Controller
    {
        private readonly IUnityOfWork IUnityOfWork;
        public CategoryController(IUnityOfWork UnityOfWork)
        {
            IUnityOfWork = UnityOfWork;
        }

        public IActionResult Index()
        {
            var objCategoryList = IUnityOfWork.Category.GetAll();
            return View(objCategoryList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "name cant match Display Order");
            }
            if (ModelState.IsValid)
            {
                IUnityOfWork.Category.Add(obj);
                IUnityOfWork.Save();
                TempData["success"] = "Adding New Category Succeded!";
                return RedirectToAction("Index", "Category");
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category categoryFromDb = IUnityOfWork.Category.Get(x => x.Id == id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            var categoryFromDb = IUnityOfWork.Category.Get(c => c.Id == obj.Id);

            if (ModelState.IsValid && (categoryFromDb.Name != obj.Name || categoryFromDb.DisplayOrder != obj.DisplayOrder))
            {
                IUnityOfWork.Category.Update(obj);
                IUnityOfWork.Save();
                TempData["success"] = "Updating Category Succeded!";

                return RedirectToAction("Index", "Category");
            }
            else if (!(categoryFromDb.Name != obj.Name || categoryFromDb.DisplayOrder != obj.DisplayOrder))
            {
                TempData["error"] = "No Changes, try to change name or Display Order !";
            }
            return View(obj);
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category categoryFromDb = IUnityOfWork.Category.Get(x => x.Id == id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category category = IUnityOfWork.Category.Get(x => x.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            IUnityOfWork.Category.Remove(category);
            IUnityOfWork.Save();
            TempData["delete"] = "Deleting Category Succeded!";

            return RedirectToAction("Index", "Category");


        }


    }
}
