using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace BookStore.Models
{
    public class BookContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<BooksPurchases> BooksPurchases { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Purchase>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Purchase>()
                .Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Book>()
                .Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Book>()
                .HasKey(b => b.Id);

            modelBuilder.Entity<BooksPurchases>()
                .Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<BooksPurchases>()
                .HasKey(b => b.Id);
            modelBuilder.Entity<BooksPurchases>()
                .HasRequired(p => p.Book);
            modelBuilder.Entity<BooksPurchases>()
                .HasRequired(p => p.Purchase);

            modelBuilder.Entity<User>()
                .HasKey(p => p.Id);
            modelBuilder.Entity<User>()
                .Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}