using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonDataItems
{
    public class Position
    {
        public int X;
        public int Y;
    }

    public class PlayerData
    {
        public string playerID;
        public string imageName = string.Empty;
       
        public string GamerTag = string.Empty;
        public string PlayerName = string.Empty;
        public int XP;
        public int GXp;
        public Position playerPosition;
        public string Password;
    }

    public class Collectable
    {
        public Position position;
        public int value;
        public int id;

        public Collectable(int id ,Position position, int value)
        {
            this.id = id;
            this.position = position;
            this.value = value;
        }
    }
}
