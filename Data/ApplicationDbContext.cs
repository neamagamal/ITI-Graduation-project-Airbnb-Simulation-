using AirBnB.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AirBnB.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<City> Cities { get; set; }
        public DbSet<Categoray> Categoraies { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<PropertyImg> PropertyImgs { get; set; }
        public DbSet<Amenity> Amenities { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<PropertyImg>(a =>
            {
                a.HasKey(a => new { a.PropertyId, a.ImgSrc });
            });
            //builder.Entity<Booking>(a =>
            //{
            //    a.HasOne(b => b.AppUser).WithMany(b => b.Bookings).HasForeignKey(a => a.AppUserId).OnDelete(DeleteBehavior.SetNull);
            //    a.HasOne(b => b.Property).WithMany(b => b.Bookings).HasForeignKey(a => a.PropertyId).OnDelete(DeleteBehavior.SetNull);
            //});
            /*builder.Entity<Review>(a =>
            {
                a.HasKey(a => new { a.PropertyId, a.AppUserId });
            });*/

            
            
        }
    }
}