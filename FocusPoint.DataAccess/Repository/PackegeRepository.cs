using FocusPoint.DataAccess.Repository.IRepository;
using FocusPoint.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusPoint.DataAccess.Repository
{
    public class PackegeRepository : Repository<Packege>, IPackegeRepository
    {
        private ApplicationDbContext _db;
        public PackegeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public void Update(Packege obj)
        {
            var objFromDb = _db.Packeges.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = obj.Name;
                objFromDb.Description = obj.Description;
                objFromDb.CreatedDateTime = obj.CreatedDateTime;
                objFromDb.Price = obj.Price;
                objFromDb.Price3 = obj.Price3;
                objFromDb.Price6 = obj.Price6;
                objFromDb.Price10 = obj.Price10;
                objFromDb.CategoryId = obj.CategoryId;

                if (obj.FileUrl != null)
                {
                    objFromDb.FileUrl = obj.FileUrl;
                }
            }
        }
    }
}
