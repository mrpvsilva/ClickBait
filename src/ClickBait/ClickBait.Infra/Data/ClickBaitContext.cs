using ClickBait.Infra.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ClickBait.Infra.Data
{
    internal class ClickBaitContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<ClickCount> ClicksCounts { get; set; }

        public ClickBaitContext(DbContextOptions<ClickBaitContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }
    }
}
