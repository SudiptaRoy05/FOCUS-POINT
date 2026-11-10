using FocusPoint.DataAccess.Repository.IRepository;
using FocusPoint.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusPoint.DataAccess.Repository
{
    public class CompositionRepository : Repository<Composition>, ICompositionRepository
    {
        private ApplicationDbContext _db;
        public CompositionRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public void Update(Composition obj)
        {
            _db.Compositions.Update(obj);
        }
    }
}
