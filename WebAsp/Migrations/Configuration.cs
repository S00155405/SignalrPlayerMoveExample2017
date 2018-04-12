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
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            context.PlayerDatas.AddOrUpdate(new CommonDataItems.PlayerData
            {
                //ID = "1234",
                PlayerName = "heck",
                XP = 12,
                //GamerTag = "something",
                //GXp = 10,
                //imageName = "picture",
                Password = "secret",
                //playerPosition = new Position()

            });



            context.SaveChanges();
        }
        
    }
}
