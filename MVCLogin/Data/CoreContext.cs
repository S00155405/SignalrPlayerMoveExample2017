using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using CommonDataItems;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace MVCLogin.Data
{
    public class CoreContext:DbContext
    {
        public DbSet<PlayerData>  Users { get; set; }

        public CoreContext()
            :base("CoreContext")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }
}