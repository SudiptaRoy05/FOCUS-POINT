using FocusPoint.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusPoint.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Composition = new CompositionRepository(_db);
            MediaItem = new MediaItemRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);
            ShoppingCart = new ShoppingCartRepository(_db);
            OrderHeader = new OrderHeaderRepository(_db);
            OrderDetail = new OrderDetailRepository(_db);
            PurchasedItem = new PurchasedItemRepository(_db);
            Packege = new PackegeRepository(_db);
            Hire = new HireRepository(_db);
            BookingHeader = new BookingHeaderRepository(_db);
            BookingDetails = new BookingDetailsRepository(_db);
            Review = new ReviewRepository(_db);
            
            
        }
        public ICategoryRepository Category { get; private set; }
        public ICompositionRepository Composition { get; private set; }
        public IMediaItemRepository MediaItem { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }
        public IOrderHeaderRepository OrderHeader { get; private set; }
        public IOrderDetailRepository OrderDetail { get; private set; }
        public IPurchasedItemRepository PurchasedItem { get; private set; }
        public IPackegeRepository Packege { get; private set; }
        public IHireRepository Hire { get; private set; }
        public IBookingDetailsRepository BookingDetails { get; private set; }
        public IBookingHeaderRepository BookingHeader { get; private set; }
        public IReviewRepository Review { get; private set; }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
