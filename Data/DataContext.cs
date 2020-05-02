using ConnectingApp.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConnectingApp.API.Data
{
    public class DataContext : DbContext // to use the context object
    {
        // to tell ef which db context is to used
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        // to add table inside db
        public DbSet<Value> Values { get; set; }

        // to add table of user
        public DbSet<User> Users { get; set; }

        // add like entity
        public DbSet<Like> Likes { get; set; }

        // now to tell efcore about many to many relationship,
        // we have to override OnModelCreating method provided by 'DbContext' class

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // first we need to set the primary key for like table
            // it'll be combination of both likerId and likeeId
            // so that 1 user cannot like another user more than once
            modelBuilder.Entity<Like>()
                .HasKey(k => new { k.LikerId, k.LikeeId });

            // now we need to efcore abput relationship

            // 1st: 1 user can be liked by many users
            modelBuilder.Entity<Like>()
                .HasOne(u => u.Likee)
                .WithMany(u => u.Likers)
                .HasForeignKey(u => u.LikeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // 1st: 1 user can like many user
            modelBuilder.Entity<Like>()
                .HasOne(u => u.Liker)
                .WithMany(u => u.Likees)
                .HasForeignKey(u => u.LikerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
