using FocusPoint.DataAccess.Repository.IRepository;
using FocusPoint.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusPoint.DataAccess.Repository
{
    public class HireRepository : Repository<Hire>, IHireRepository
    {
        private ApplicationDbContext _db;
        public HireRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public int DecrementCount(Hire hire, int count)
        {
            hire.Days -= count;
            return hire.Days;
        }

        public int IncrementCount(Hire hire, int count)
        {
            hire.Days += count;
            return hire.Days;
        }
    }
}
