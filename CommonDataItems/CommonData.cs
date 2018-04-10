using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

    [Table("PlayerDetails")]
    public class PlayerData
    {
        
        [Key]
        public string ID { get; set; }
        [Display(Name = "Image Name")]
        public string imageName = string.Empty;
        [Display(Name = "Gamertag")]
        public string GamerTag = string.Empty;
        [Display(Name = "player Name")]
        public string PlayerName { get; set; }
        [Display(Name = "XP")]
        public int XP { get; set; }
        public int GXp;
        public Position playerPosition;
        [Display(Name = "Password")]
        public string Password { get; set; }
    }

    public class Collectable
    {
        public Position position;
        public int value;
        public int id;
        public bool alive = true;

        public Collectable(int id ,Position position, int value)
        {
            this.id = id;
            this.position = position;
            this.value = value;
        }
    }
}
