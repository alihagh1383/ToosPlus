using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class NewsDbContext : DbContext
    {
        public NewsDbContext(DbContextOptions<NewsDbContext> options) : base(options) { }
        public DbSet<News> News { get; set; }
        public DbSet<NewsCategory> NewsCategories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Slide> Slides { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<News>().HasMany(p => p.Likes).WithMany(p => p.LikedNews);
            modelBuilder.Entity<User>().HasMany(p => p.News).WithOne(p => p.Creator);
            base.OnModelCreating(modelBuilder);
        }
    }
}
