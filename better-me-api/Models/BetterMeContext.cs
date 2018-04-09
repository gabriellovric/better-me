using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace BetterMeApi.Models
{
    public class BetterMeContext : DbContext
    {
        public BetterMeContext(DbContextOptions<BetterMeContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(entity => {
                entity.HasIndex(u => u.Email).IsUnique();
            });
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Progress> Progresses { get; set; }
    }
}