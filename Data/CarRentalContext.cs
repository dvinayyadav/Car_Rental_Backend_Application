using Car_Rental_Backend_Application.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Car_Rental_Backend_Application.Data
{
    public class CarRentalContext : DbContext
    {
        public CarRentalContext(DbContextOptions<CarRentalContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Admin> Admin { get; set; }
        public DbSet<Cancellation> Cancellations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Phone_Number)
                .IsUnique();

            modelBuilder.Entity<Admin>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.User_ID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Car)
                .WithMany(c => c.Bookings)
                .HasForeignKey(b => b.Car_ID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cancellation>()
                .HasOne(c => c.Booking)
                .WithMany(b => b.Cancellations)
                .HasForeignKey(c => c.Booking_ID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Admin>().HasData(
       new Admin
       {
           Admin_ID = 1,
           Username = "devaravinay698",
           Email = "devaravinay698.com",
           Password = "Vinay@123", 
       },
       new Admin
       {
           Admin_ID = 2,
           Username = "narasimhagorla45",
           Email = "narasimhagorla45@gmail.com",
           Password = "Narasimha@123",
       },
           new Admin
           {
               Admin_ID = 3,
               Username = "rupeshsanagala523",
               Email = "rupeshsanagala523@gmail.com",
               Password = "Rupesh@123",
           }
           ,
           new Admin
           {
               Admin_ID = 4,
               Username = "ajaythella0",
               Email = "ajaythella0@gmail.com",
               Password = "Ajay@123",
           }
   );

            base.OnModelCreating(modelBuilder);
        }
    }
}
