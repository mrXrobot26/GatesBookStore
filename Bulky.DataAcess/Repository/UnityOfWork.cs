using Bulky.DataAcess.Repository.IRepository;
using Bulky.DataAcess.Repository.IRepsitory;
using Bulky.Models;
using BulkyAcess.DataAcess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAcess.Repository
{
    public class UnityOfWork : IUnityOfWork
    {
        private ApplicationDbContext _db;
        public ICategoryRepository Category { get; private set; }
        public IProductRepository product { get; private set; }
        public ICompanyRepository company { get; private set; }
        public IShoppingCartRepository ShoppingCart {  get; private set; }
        public IApplicationUserRepository ApplicationUser{get; private set; }
        public IOrderDetailRepository OrderDetail { get; private set; }
        public IOrderHeaderRepository OrderHeader { get; private set; }


        public UnityOfWork(ApplicationDbContext db)
        {
            _db = db;
            OrderDetail = new OrderDetailRepository(_db);
            OrderHeader = new OrderHeaderRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);
            ShoppingCart = new ShoppingCartRepository(_db);
            Category = new CategoryRepository(_db);
            product = new ProductRepository(_db);
            company = new CompanyRepository(_db);   
        }
        public void Save()
        {
            _db.SaveChanges();
        }

    }
}
