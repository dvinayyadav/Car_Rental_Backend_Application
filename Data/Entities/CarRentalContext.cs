using Microsoft.EntityFrameworkCore;

namespace Car_Rental_Backend_Application.Data.Entities
{
    public class CarRentalContext : DbContext
    {

        public CarRentalContext(DbContextOptions<CarRentalContext> options) : base(options) { }

       
        public DbSet<User> Users { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Admin> Admin{ get; set; }
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
                .OnDelete(DeleteBehavior.Cascade);  
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

      
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
