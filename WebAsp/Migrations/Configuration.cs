namespace WebAsp.Migrations
{
    using CommonDataItems;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<WebAsp.Models.WebAspContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(WebAsp.Models.WebAspContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            context.PlayerDatas.AddOrUpdate(new PlayerData
            {
                ID = "1234",
                PlayerName = "heck",
                XP = 12,
                //GamerTag = "something",
                //GXp = 10,
                //imageName = "picture",
                Password = "secret",
                //playerPosition = new Position()
                
            });
            //context.PlayerDatas.AddOrUpdate(new PlayerData
            //{
            //    ID = "3456",
            //    PlayerName = "person",
            //    XP = 15,
                
            //    //GamerTag = "something else",
            //    //GXp = 1,
            //    //imageName = "pictur",
            //    Password = "secret2",
            //    //playerPosition = new Position()
            //});
            context.SaveChanges();
        }
    }
}
