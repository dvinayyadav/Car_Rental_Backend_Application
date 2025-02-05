using Microsoft.EntityFrameworkCore;

namespace Car_Rental_Backend_Application.Data.Entities
{
    public class CarRentalContext : DbContext
    {

        public CarRentalContext(DbContextOptions<CarRentalContext> options) : base(options) { }

        // DbSets for each entity
        public DbSet<User> Users { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Admin> Admin{ get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Cancellation> Cancellations { get; set; }
        public DbSet<Availability> Availabilities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Car>()
        .Property(c => c.Availability_Status)
        .HasConversion<string>(); // Store as string in DB



            modelBuilder.Entity<User>()
               .HasIndex(u => u.Email)
               .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Phone_Number)
                .IsUnique();

            modelBuilder.Entity<Admin>()
              .HasIndex(u => u.Email)
              .IsUnique();

            // Configure relationships for Booking
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.User_ID)
                .OnDelete(DeleteBehavior.Cascade);  // Cascade delete when User is deleted

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Car)
                .WithMany(c => c.Bookings)
                .HasForeignKey(b => b.Car_ID)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent delete of Car if it's associated with a booking

            // Configure relationships for Reservation
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reservations)
                .HasForeignKey(r => r.User_ID)
                .OnDelete(DeleteBehavior.Cascade);  // Cascade delete when User is deleted

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Car)
                .WithMany(c => c.Reservations)
                .HasForeignKey(r => r.Car_ID)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent delete of Car if it's associated with a reservation

            // Configure relationships for Cancellation
            modelBuilder.Entity<Cancellation>()
                .HasOne(c => c.Booking)
                .WithMany(b => b.Cancellations)
                .HasForeignKey(c => c.Booking_ID)
                .OnDelete(DeleteBehavior.Cascade);  // Cascade delete when Booking is deleted

            // Configure relationships for Availability
            modelBuilder.Entity<Availability>()
                .HasOne(a => a.Car)
                .WithOne(c => c.Availability)  // Assuming Car has Availability navigation property
                .HasForeignKey<Availability>(a => a.Car_ID)  // FK from Availability to Car
                .OnDelete(DeleteBehavior.Cascade);  // Cascade delete when Car is deleted

            base.OnModelCreating(modelBuilder);
        }
    }
}
