using FocusPoint.DataAccess.Repository.IRepository;
using FocusPoint.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusPoint.DataAccess.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private ApplicationDbContext _db;
        public ApplicationUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void GetUserById(string UsertId)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.Id == UsertId);
        }

        public void Update(ApplicationUser obj)
        {
            var objFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = obj.Name;
                objFromDb.PhoneNumber = obj.PhoneNumber;
                objFromDb.Email = obj.Email;
                objFromDb.Address = obj.Address;
                objFromDb.City = obj.City;
                objFromDb.PostalCode = obj.PostalCode;
                objFromDb.District = obj.District;
                objFromDb.Type = obj.Type;
                
                if (obj.ProfilePic != null)
                {
                    objFromDb.ProfilePic = obj.ProfilePic;
                }
            }
        }
    }
}
