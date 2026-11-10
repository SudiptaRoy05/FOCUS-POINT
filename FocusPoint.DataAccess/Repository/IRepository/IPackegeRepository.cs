using FocusPoint.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusPoint.DataAccess.Repository.IRepository
{
    public interface IPackegeRepository : IRepository<Packege>
    {
        public void Update(Packege obj);
    }
}
