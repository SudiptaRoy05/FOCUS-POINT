using FocusPoint.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusPoint.DataAccess.Repository.IRepository
{
    public interface IHireRepository : IRepository<Hire>
    {
        int IncrementCount(Hire hire, int count);
        int DecrementCount(Hire hire, int count);
    }
}
