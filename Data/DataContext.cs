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
    }
}
