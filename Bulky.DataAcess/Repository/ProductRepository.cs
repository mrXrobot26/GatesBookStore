using Bulky.DataAcess.Repository.IRepsitory;
using Bulky.Models;
using BulkyAcess.DataAcess.Data;
using BulkyAcess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAcess.Repository
{
    public class ProductRepository : Repsitory<Product>,IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) :base(db)
        {
            _db = db;
        }
        public void Update(Product obj)
        {

            //_db.products.Update(obj);
            var objFromDb = _db.products.FirstOrDefault(p => p.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Title = obj.Title;
                objFromDb.ISBN = obj.ISBN;
                objFromDb.Price = obj.Price;
                objFromDb.Price50 = obj.Price50;
                objFromDb.ListPrice = obj.ListPrice;
                objFromDb.Price100 = obj.Price100;
                objFromDb.Description = obj.Description;
                objFromDb.CategoryId = obj.CategoryId;
                objFromDb.Author = obj.Author;
                if (obj.ImgageUrl != null)
                {
                    objFromDb.ImgageUrl = obj.ImgageUrl;
                }
            }

        }
    }
}
