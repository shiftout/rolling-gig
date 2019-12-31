using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RollingGigApi.Models
{
    public class RollingGigContext : DbContext
    {
        public RollingGigContext(DbContextOptions<RollingGigContext> options)
            : base(options)
        {
        }

        public DbSet<Tag> Tags { get; set; }
        public DbSet<TodoItem> TodoItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoItemTag>()
                .HasKey(t => new { t.TodoItemId, t.TagId });

            modelBuilder.Entity<TodoItemTag>()
                .HasOne(pt => pt.TodoItem)
                .WithMany(p => p.TodoItemTags)
                .HasForeignKey(pt => pt.TodoItemId);

            modelBuilder.Entity<TodoItemTag>()
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.TodoItemTags)
                .HasForeignKey(pt => pt.TagId);
        }
    }
}
