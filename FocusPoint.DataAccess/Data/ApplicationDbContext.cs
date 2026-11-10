using FocusPoint.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FocusPoint.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options):base(options) 
        {
            
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Composition> Compositions { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<MediaItem> MediaItems { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set;}
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<PurchasedItem> PurchasedItems { get; set; }
        public DbSet<Packege> Packeges { get; set; }
        public DbSet<Hire> Hires { get; set; }
        public DbSet<BookingHeder> BookingHeders { get; set; }
        public DbSet<BookingDetails> BookingDetails { get; set; }
        public DbSet<Review> Reviews { get; set; }
        
    }
}
