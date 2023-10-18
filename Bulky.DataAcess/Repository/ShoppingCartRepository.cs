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
    public class ShoppingCartRepository : Repsitory<ShoppingCart>, IShoppingCartRepository
    {
        private  ApplicationDbContext _db;
        public ShoppingCartRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ShoppingCart obj)
        {
            _db.ShoppingCarts.Update(obj);
        }
    }
}
