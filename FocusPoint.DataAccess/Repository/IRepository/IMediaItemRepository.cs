using FocusPoint.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusPoint.DataAccess.Repository.IRepository
{
    public interface IMediaItemRepository : IRepository<MediaItem>
    {
        public void Update(MediaItem obj);

        public void Delete(MediaItem obj);
    }
}
