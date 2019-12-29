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
    }
}
