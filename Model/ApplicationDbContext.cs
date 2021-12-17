using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Model
{
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<OS_User> OS_Users { get; set; }
        public DbSet<OS_Admin> OS_Admins { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<OS_User>(b =>
            {
                b.ToTable("OS_User");
            });
            modelBuilder.Entity<OS_Admin>(b =>
            {
                b.ToTable("OS_Admin");
            });

        }
    }
}
