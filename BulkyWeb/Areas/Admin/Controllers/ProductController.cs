using Bulky.DataAcess.Repository;
using Bulky.DataAcess.Repository.IRepsitory;
using Bulky.Models;
using Bulky.Models.ViewModel;
using BulkyAcess.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.IdentityModel.Tokens;
using System.IO;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using BulkyAcess.Utility;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]

    public class ProductController : Controller
    {
        private readonly IUnityOfWork _unityOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnityOfWork unityOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unityOfWork = unityOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var productList = _unityOfWork.product.GetAll(includeProperties:"Category");
            
            return View(productList);
        }
        // i have question here where it get id??!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {

                CategoryList = _unityOfWork.Category.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                product = new Product()

            };
            if (id==null || id == 0)
            {
                //create
                return View(productVM);

            }
            else
            {
                //update
                productVM.product = _unityOfWork.product.Get(x=>x.Id==id);
                return View(productVM);

            }
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM , IFormFile?file) 
        {

            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file!=null)
                {
                    var fileName = Guid.NewGuid().ToString()+Path.GetExtension(file.FileName);
                    var productPath = Path.Combine(wwwRootPath, @"Images\Products\");
                    if(!string.IsNullOrEmpty(productVM.product.ImgageUrl))//if it has url it means that update , ididnt add imgurl yet
                    {
                        //oldimg delate
                        var oldImg = Path.Combine(wwwRootPath, productVM.product.ImgageUrl.TrimStart('\\'));
                        if(System.IO.File.Exists(oldImg)) 
                        { 
                            System.IO.File.Delete(oldImg);
                        }

                    }
                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.product.ImgageUrl = @"\Images\Products\" + fileName;

                }
                if (productVM.product.Id==0)
                {
                _unityOfWork.product.Add(productVM.product);

                }
                else
                {
                    _unityOfWork.product.Update(productVM.product);
                }
                _unityOfWork.Save();
                TempData["success"] = "Adding New product Succeded!";
                return RedirectToAction("Index", "Product");
            }
            else // error
            {
                productVM.CategoryList = _unityOfWork.Category.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });
                return View(productVM);
            }
        }
      
        
        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            // to use datatabl
            List<Product> objProductList = _unityOfWork.product.GetAll(includeProperties: "Category").ToList();
            return Json(new {data = objProductList});
        }

        [HttpDelete]
        public IActionResult Delete(int?id)
        {
            var productToDelete = _unityOfWork.product.Get(x => x.Id == id);
            if (productToDelete == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            var oldImagePath =
                           Path.Combine(_webHostEnvironment.WebRootPath,
                           productToDelete.ImgageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            _unityOfWork.product.Remove(productToDelete);
            _unityOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });

        }

        #endregion


    }
}
