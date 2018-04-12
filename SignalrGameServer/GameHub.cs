using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using CommonDataItems;
using System.Net;
using Newtonsoft.Json;

namespace SignalrGameServer
{

    public class GameHub : Hub
    {
        
        public int CollID = 0;
        #region Game hub variables
        public static int TPlayer = 0;
        Collectable collectable;
        public static List<Collectable> GameCollectables = new List<Collectable>();
        

        // Use static to protect Data across dofferent hub invocations
        public static Queue<PlayerData> RegisteredPlayers = new Queue<PlayerData>(new PlayerData[]
        {

            //new PlayerData { GamerTag = "Dark Terror", imageName = "", ID = Guid.NewGuid().ToString(), XP = 200 ,GXp = 0 },
            //new PlayerData { GamerTag = "Mistic Meg", imageName = "", ID = Guid.NewGuid().ToString(), XP = 2000,GXp = 0  },
            //new PlayerData { GamerTag = "Jinxy", imageName = "", ID = Guid.NewGuid().ToString(), XP = 1200 ,GXp = 0 },
            //new PlayerData { GamerTag = "Jabber Jaws", imageName = "", ID = Guid.NewGuid().ToString(), XP = 3200,GXp = 0  },
            //new PlayerData { GamerTag = "Darks Terror", imageName = "", ID = Guid.NewGuid().ToString(), XP = 200,GXp = 0  },
            //new PlayerData { GamerTag = "Mistics Meg", imageName = "", ID = Guid.NewGuid().ToString(), XP = 2000,GXp = 0  },
            //new PlayerData { GamerTag = "Jinxys", imageName = "", ID = Guid.NewGuid().ToString(), XP = 1200,GXp = 0  },
            //new PlayerData { GamerTag = "Jabbers Jaws", imageName = "", ID = Guid.NewGuid().ToString(), XP = 3200,GXp = 0  },
        });
       
        public static List<PlayerData> Players = new List<PlayerData>();

        public static Stack<string> characters = new Stack<string>(
                    new string[] { "Player 8", "Player 7", "Player 6", "Player 5", "Player 4", "Player 3", "Player 2", "Player 1" });

        #endregion
    
        public void Hello()
        {
            Clients.All.hello();
            //Players.Clear();
            //Clients.All.Remove();
        }


        public PlayerData Join(PlayerData name)
        {
            //Clients.All.Remove();
            // Check and if the charcters
            if (characters.Count > 0)
            {
                // pop name
                string character = characters.Pop();
                // if there is a registered player
                if (name != null)
                {


                    PlayerData newPlayer = new PlayerData { GamerTag = name.PlayerName, imageName = "", ID = name.ID, XP = name.XP, GXp = 0 };
                    newPlayer.imageName = character;
                    newPlayer.playerPosition = new Position { X = new Random().Next(0,700),Y = new Random().Next(0,20) };
                    // Tell all the other clients that this player has Joined
                    Clients.Others.Joined(newPlayer);
                    // Tell this client about all the other current 
                    Clients.Caller.CurrentPlayers(Players);
                    // Finaly add the new player on teh server
                    Players.Add(newPlayer);
                    return newPlayer;
                }
               
                
            }
            return null;
        }

        public List<Collectable> spawnCollectable()
        {
            
            if (TPlayer <= 1)
            {
                Random ran = new Random();
                //List<Collectable> collectables = new List<Collectable>();
               // GameCollectables.Clear();

                for (int i = 0; i <= 9; i++)
                {

                    collectable = new Collectable(CollID, new Position { X = ran.Next(0, 700), Y = ran.Next(100, 400) }, ran.Next(10, 50));
                    GameCollectables.Add(collectable);
                    CollID++;

                }

                //Clients.Others.DrawCollectables(GameCollectables);
                Clients.Caller.DrawCollectables(GameCollectables);
                //GameCollectables2 = GameCollectables;
                TPlayer++; 
            }
            else
            {

                Clients.Caller.DrawCollectables(GameCollectables);
            }
            return GameCollectables;
        }

        public int RemoveColl(Collectable col, PlayerData play)
        {
            Random ran = new Random();
            List<Collectable> GameCollectables2 = new List<Collectable>();
            Collectable found = GameCollectables.FirstOrDefault(g => g.id == col.id);
            if (found == null)
            {

            }
            else
            {
                PlayerData foundPlayer = Players.FirstOrDefault(p => p.ID == play.ID);

                foundPlayer.GXp += col.value;

                found.alive = false;
                
            }

            int count = 0;
            foreach (Collectable c in GameCollectables)
            {
                if (!c.alive)
                {
                    count++;
                }
            }
            if (count == GameCollectables.Count)
            {
                //proxy.Invoke("RemoveColl", new object[] { CollectedBox, Player });
                //End Game
                DisplayLeaderBoard();
            }

            return found.id;

        }

        private void DisplayLeaderBoard()
        {
            foreach (PlayerData item in Players)
            {

                Clients.All.AddToLeaderboard(item);

            }
            foreach(PlayerData item in Players)
            {
                item.XP += item.GXp;
                using (WebClient client = new WebClient())
                {
                    string jsonData = client.UploadString("http://localhost:63207/api/PlayerDatas/5", item.ToString());
                    
                }
            }

        }

        public void Moved(string playerID, Position newPosition, int score)
        {
            // Update the collection with the new player position is the player exists
            PlayerData found = Players.FirstOrDefault(p => p.ID == playerID);

            if (found != null)
            {
                // Update the server player position
                found.playerPosition = newPosition;
                // Tell all the other clients this player has moved
                Clients.Others.OtherMove(playerID,newPosition);
            }
            
        }

        public void RemovePlayer(string playerID)
        {
            // Update the collection with the new player position is the player exists
            PlayerData found = Players.FirstOrDefault(p => p.ID == playerID);

            if (found != null)
            {

                // Tell this client about all the other current 
                Clients.Caller.CurrentPlayers(Players);
                // Finaly add the new player on teh server
                Players.Remove(found);
                characters.Push(found.imageName);
                Clients.Others.Left(found);
            }

        }
    }
}