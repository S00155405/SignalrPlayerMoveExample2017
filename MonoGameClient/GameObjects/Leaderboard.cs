using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameClient.GameObjects
{
    class Leaderboard : DrawableGameComponent
    {
        //Sets leaderboard text 
            Vector2 basePosition;

            public Leaderboard(Game game) : base(game)
            {
                game.Components.Add(this);
                basePosition = new Vector2(game.GraphicsDevice.Viewport.Width - 800, game.GraphicsDevice.Viewport.Height - 200);
            }
            protected override void LoadContent()
            {

                base.LoadContent();
            }



            public override void Update(GameTime gameTime)
            {

                var Stack = Game.Components.Where(
                                t => t.GetType() == typeof(LeaderboardText));
                if (Stack.Count() > 0)
                {
                    Vector2 b = basePosition;
                    var font = Game.Services.GetService<SpriteFont>();
                    Vector2 fontsize = font.MeasureString("Y");
                    foreach (LeaderboardText ft in Stack)
                    {
                        ft.Position = b;
                        b += new Vector2(0, fontsize.Y + 10);
                    }
                }
                base.Update(gameTime);
            }


        }


        class LeaderboardText : DrawableGameComponent
        {
            string text;
            Vector2 position;
            byte blend = 255;

            public Vector2 Position
            {
                get
                {
                    return position;
                }

                set
                {
                    position = value;
                }
            }

            public LeaderboardText(Game game, Vector2 Position, string Text) : base(game)
            {
                game.Components.Add(this);
                text = Text;
                this.Position = Position;
            }

            public override void Update(GameTime gameTime)
            {
                var Stack = Game.Components.Where(
                               t => t.GetType() == typeof(LeaderboardText));
                if (Stack.Count() > 5)
                {
                    //if (blend > 0)
                    //    blend--;
                    //else { Game.Components.Remove(this); }

                    Game.Components.Remove(this);
                }

                base.Update(gameTime);
            }

            public override void Draw(GameTime gameTime)
            {
                var sp = Game.Services.GetService<SpriteBatch>();
                var font = Game.Services.GetService<SpriteFont>();
                Color myColor = new Color((byte)0, (byte)0, (byte)0, blend);
                sp.Begin(SpriteSortMode.Immediate, BlendState.Additive);
                sp.DrawString(font, text, Position, new Color((byte)255, (byte)255, (byte)255, blend), 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                sp.End();
                base.Draw(gameTime);
            }

        }
    }


