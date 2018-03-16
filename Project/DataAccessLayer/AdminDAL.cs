using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Project.Models;

namespace Project.DataAccessLayer
{
    //Data access layer for admin
    public class AdminDAL : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<AdminDAL>(null);
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Admin>().ToTable("tblAdmin");
        }
        public DbSet<Admin> Admins { get; set; }
    }
}