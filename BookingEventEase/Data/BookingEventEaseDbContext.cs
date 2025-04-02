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
        public DbSet<Bookings> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

            base.OnModelCreating(modelBuilder);
        }
    }
}
