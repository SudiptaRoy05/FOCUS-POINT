using FocusPoint.DataAccess.Repository.IRepository;
using FocusPoint.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusPoint.DataAccess.Repository
{
    public class PurchasedItemRepository : Repository<PurchasedItem>, IPurchasedItemRepository
    {
        private ApplicationDbContext _db;
        public PurchasedItemRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public void Update(PurchasedItem obj)
        {
            _db.PurchasedItems.Update(obj);
        }
    }
}
