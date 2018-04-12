using CommonDataItems;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;


namespace MVCLogin.Data
{
    public class ContextInitialiser: DropCreateDatabaseIfModelChanges<CoreContext>
    {
        protected override void Seed(CoreContext context)
        {
            var users = new List<PlayerData>
            { new PlayerData() {PlayerName="Joe",XP=0,Password="fffyiuyuyi"} };
            users.ForEach(u => context.Users.Add(u));
            base.Seed(context);
        }
    }
}