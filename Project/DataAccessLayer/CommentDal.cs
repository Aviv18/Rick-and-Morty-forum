using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Project.Models;

namespace Project.DataAccessLayer
{
    //Data access layer for comments
    public class CommentDal : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<CommentDal>(null);
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Comment>().ToTable("tblComment");
        }
        public DbSet<Comment> Comments { get; set; }

    }
}