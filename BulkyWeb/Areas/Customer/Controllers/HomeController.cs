
using Bulky.DataAcess.Migrations;
using Bulky.DataAcess.Repository;
using Bulky.DataAcess.Repository.IRepsitory;
using Bulky.Models;
using BulkyAcess.Utility.ErrorViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BulkyWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnityOfWork _unityOfWork;

        public HomeController(ILogger<HomeController> logger,IUnityOfWork unityOfWork)
        {
            _logger = logger;
            _unityOfWork = unityOfWork;
        }

        public IActionResult Index()
        {
            var ProductList = _unityOfWork.product.GetAll(includeProperties:"Category");

            return View(ProductList);
        }
        public IActionResult Detials(int id)
        {
            ShoppingCart cart = new()
            {
                Product = _unityOfWork.product.Get(u => u.Id == id, includeProperties: "Category"),
                ProductId = id,
                Count = 1
            };
            return View(cart);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Detials(ShoppingCart shoppingCart)
        {
            shoppingCart.Id = 0;
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId = userId;
            
            ShoppingCart cartFromDb = _unityOfWork.ShoppingCart.Get(x=> x.ApplicationUserId == userId && x.ProductId==shoppingCart.ProductId);
            if (cartFromDb!=null)
            {
                cartFromDb.Count += shoppingCart.Count;
                _unityOfWork.ShoppingCart.Update(cartFromDb);
            }
            else
            {
            _unityOfWork.ShoppingCart.Add(shoppingCart);
            }

            _unityOfWork.Save();

            return RedirectToAction(nameof(Index));
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
