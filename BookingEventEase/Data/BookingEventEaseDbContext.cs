using BookingEventEase.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingEventEase.Data
{
    public class BookingEventEaseDbContext : DbContext
    {
        public BookingEventEaseDbContext(DbContextOptions<BookingEventEaseDbContext> options) : base(options)
        { }
        public DbSet<Event> Events { get; set; }
        public DbSet<Venue> Venue { get; set; }
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<Bookings> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /* 
              modelBuilder.Entity<Bookings>()
                  .HasOne(b => b.Venue)
                  .WithMany()
                  .HasForeignKey(b => b.VenueId)
                  .OnDelete(DeleteBehavior.Restrict); // Prevents cascade delete

              modelBuilder.Entity<Bookings>()
                  .HasOne(b => b.Event)
                  .WithMany()
                  .HasForeignKey(b => b.EventID)
                  .OnDelete(DeleteBehavior.Restrict); // Prevents cascade delete
           

            // New update
            modelBuilder.Entity<Bookings>()
            .HasIndex(b => new { b.EventID, b.VenueId, b.BookingDate })
            .IsUnique(); // Prevent double booking
            base.OnModelCreating(modelBuilder);
            */  // comments

            base.OnModelCreating(modelBuilder);

            // Prevent double bookings for the same venue on the same date and event
            modelBuilder.Entity<Bookings>()
                .HasIndex(b => new { b.EventID, b.VenueId, b.BookingDate })
                .IsUnique();

            // Configure foreign key to Event with restricted delete behavior
            modelBuilder.Entity<Bookings>()
                .HasOne(b => b.Event)
                .WithMany()
                .HasForeignKey(b => b.EventID)
                .OnDelete(DeleteBehavior.Restrict); // Prevents cascade delete

            // Configure foreign key to Venue with restricted delete behavior
            modelBuilder.Entity<Bookings>()
                .HasOne(b => b.Venue)
                .WithMany()
                .HasForeignKey(b => b.VenueId)
                .OnDelete(DeleteBehavior.Restrict); // Prevents cascade delete

            // Seed a default EventType to satisfy existing Events
            modelBuilder.Entity<EventType>().HasData(
                new EventType { EventTypeId = 1, EventTypeName = "General" }
            );
        }
    }
}
