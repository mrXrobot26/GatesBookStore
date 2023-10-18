using Bulky.DataAcess.Repository.IRepsitory;
using Bulky.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BulkyAcess.Utility;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnityOfWork _UnityOfWork;
        public CompanyController(IUnityOfWork UnityOfWork)
        {
            _UnityOfWork = UnityOfWork;
        }

        public IActionResult Index()
        {
            var companies = _UnityOfWork.company.GetAll();
            return View(companies);
        }
        public IActionResult Upsert(int?id)
        {
            Company company;
            if (id==null || id==0)//Create
            {
                company = new Company();
            }
            else
            {
                company = _UnityOfWork.company.Get(x => x.Id == id);

            }
            return View(company);

        }
        [HttpPost]
        public IActionResult Upsert(Company companyObj)
        {
            if (ModelState.IsValid)
            {
                if (companyObj.Id==0)
                {
                    _UnityOfWork.company.Add(companyObj);
                    TempData["success"] = "Company created successfully";
                }
                else
                {
                    _UnityOfWork.company.Update(companyObj);
                    TempData["success"] = "Company Updated successfully";

                }
                _UnityOfWork.Save();
                return RedirectToAction("Index");
            }
            else
            {
                return View(companyObj);
            }

        }
        public IActionResult GetAll()
        {
            var companies = _UnityOfWork.company.GetAll();
            return Json(new { data= companies });
        }
        [HttpDelete]
        public IActionResult Delete(int id) 
        {

            var companyToBeDeleted = _UnityOfWork.company.Get(x=>x.Id==id);
            if (companyToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _UnityOfWork.company.Remove(companyToBeDeleted);
            _UnityOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });

        }
    }
}
