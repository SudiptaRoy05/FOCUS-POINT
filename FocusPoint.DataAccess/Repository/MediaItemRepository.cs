using FocusPoint.DataAccess.Repository.IRepository;
using FocusPoint.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusPoint.DataAccess.Repository
{
    public class MediaItemRepository : Repository<MediaItem>, IMediaItemRepository
    {
        private ApplicationDbContext _db;
        public MediaItemRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Delete(MediaItem obj)
        {
            var objFromDb = _db.MediaItems.FirstOrDefault(u => u.Id == obj.Id);
            obj.IsPublic = false;   
        }

        public void Update(MediaItem obj)
        {
            var objFromDb = _db.MediaItems.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.FileName = obj.FileName;
                objFromDb.Description = obj.Description;
                objFromDb.UploadDate = obj.UploadDate;
                objFromDb.Price = obj.Price;
                objFromDb.Type = obj.Type;
                objFromDb.IsPublic = obj.IsPublic;
                objFromDb.CategoryId = obj.CategoryId;
                objFromDb.CompositionId = obj.CompositionId;
                if (obj.FileUrl != null)
                {
                    objFromDb.FileUrl = obj.FileUrl;
                }
            }
        }
    }
}
