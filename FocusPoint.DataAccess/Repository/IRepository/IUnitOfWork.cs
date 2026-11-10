using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusPoint.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        ICompositionRepository Composition { get; }
        IMediaItemRepository MediaItem { get; }
        IApplicationUserRepository ApplicationUser { get; }
        IShoppingCartRepository ShoppingCart { get; }
        IOrderHeaderRepository OrderHeader { get; }
        IOrderDetailRepository OrderDetail { get; }
        IPurchasedItemRepository PurchasedItem { get; }
        IPackegeRepository Packege { get; }
        IHireRepository Hire { get; }
        IBookingHeaderRepository BookingHeader { get; }
        IBookingDetailsRepository BookingDetails { get; }
        IReviewRepository Review { get; }

        public void Save();
    }
}
