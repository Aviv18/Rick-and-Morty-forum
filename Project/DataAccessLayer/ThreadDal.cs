using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Project.Models;

namespace Project.DataAccessLayer
{
    //Data access layer for threads
    public class ThreadDal : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<ThreadDal>(null);
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Thread>().ToTable("tblThread");
        }
        public DbSet<Thread> Threads { get; set; }
    }
}