namespace DotNetCoreKit.Models
{
    using DotNetCoreKit.Models;
    using Microsoft.EntityFrameworkCore;

    public class PeopleContext : DbContext
    {
        public PeopleContext(DbContextOptions<PeopleContext> options) : base(options) { }

        public DbSet<People> People { get; set; }

    }
}
