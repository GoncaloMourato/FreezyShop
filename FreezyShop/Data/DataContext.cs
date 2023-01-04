using FreezyShop.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FreezyShop.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<InfoClient> InfoClients { get; set; }
        public DbSet<ProductCategory> ProductsCategories { get; set; }

        public DbSet<PaymentMethod> PaymentMethods { get; set; }

        public DbSet<ShippingInfo> ShippingInfos { get; set; }

        public DbSet<Size> Sizes { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<Return> Returns { get; set; }

        public DbSet<ProductSizes> ProductSizes { get; set; }

        public DbSet<ClientPreference> ClientPreferences { get; set; }

        public DbSet<PromoCode> PromoCodes { get; set; }

        public DbSet<ClientFavourite> ClientFavourites { get; set; }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    var cascadeFKs = modelBuilder.Model
        //        .GetEntityTypes()
        //        .SelectMany(t => t.GetForeignKeys())
        //        .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

        //    foreach (var fk in cascadeFKs)
        //    {
        //        fk.DeleteBehavior = DeleteBehavior.Restrict;
        //    }

        //    base.OnModelCreating(modelBuilder);
        //}
    }
}
