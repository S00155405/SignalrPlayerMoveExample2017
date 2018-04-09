using System;
using CommonDataItems;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Engine.Engines;
using Sprites;
using System.Collections.Generic;
using GameComponentNS;
using gameClient.GameObjects;

namespace MonoGameClient
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        string connectionMessage = string.Empty;
        PlayerData Player;
        Collectable collectable;
        Texture2D collTexture;
        Texture2D PlayerTex;
        public bool DrawC = false;
        public static List<PlayerData> totalPlayers = new List<PlayerData>();
        public static List<Collectable> totalCollectable = new List<Collectable>();
        // SignalR Client object delarations

        HubConnection serverConnection;
        IHubProxy proxy;

        public bool Connected { get; private set; }
        public float Timer = 3;

        Random randomPositionGenerator = new Random();
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // create input engine
            new InputEngine(this);
            new FadeTextManager(this);
            new Leaderboard(this);

            // TODO: Add your initialization logic here change local host to newly created local host
            // http://signalrgameserver20171123102038.azurewebsites.net/
            // Second server http://ppowellgameserver.azurewebsites.net
            serverConnection = new HubConnection("http://localhost:53922/");
            //serverConnection = new HubConnection("https://rad302gameass.azurewebsites.net");
            //serverConnection = new HubConnection("http://ppowellgameserver.azurewebsites.net");
            serverConnection.StateChanged += ServerConnection_StateChanged;
            proxy = serverConnection.CreateHubProxy("GameHub");
            serverConnection.Start();

            Action<PlayerData> DL = DisplayLeaderBoard;
            proxy.On<PlayerData>("AddToLeaderboard", DL);

            //Action<Collectable> RCol = RemoveCollectable;
            //proxy.On<Collectable>("RemoveCollectable", RCol);

            Action<List<Collectable>> blah = CreateCollectables;
            proxy.On<List<Collectable>>("DrawCollectables", blah);

            Action<PlayerData> joined = clientJoined;
            proxy.On<PlayerData>("Joined", joined);

            Action<List<PlayerData>> currentPlayers = clientPlayers;
            proxy.On<List<PlayerData>>("CurrentPlayers", currentPlayers);

            Action<string, Position> otherMove = clientOtherMoved;
            proxy.On<string, Position>("OtherMove", otherMove);

           

            // Add the proxy client as a Game service o components can send messages 
            Services.AddService<IHubProxy>(proxy);



            base.Initialize();
        }

        private void DisplayLeaderBoard(PlayerData obj)
        {
            new LeaderboardText(this, new Vector2 (GraphicsDevice.Viewport.Width/2, GraphicsDevice.Viewport.Height/2), string.Format(" {0}  {1} + {2}",
                         obj.GamerTag , obj.XP, obj.GXp ));
        }
        
        private void clientOtherMoved(string playerID, Position newPos)
        {
            // iterate over all the other player components 
            // and check to see the type and the right id
            foreach (var player in Components)
            {
                if (player.GetType() == typeof(OtherPlayerSprite)
                    && ((OtherPlayerSprite)player).pData.playerID == playerID)
                {
                    OtherPlayerSprite p = ((OtherPlayerSprite)player);
                    p.pData.playerPosition = newPos;
                    p.Target = new Point(p.pData.playerPosition.X, p.pData.playerPosition.Y);
                    break; // break out of loop as only one player position is being updated
                           // and we have found it
                }
            }
        }
        // Only called when the client joins a game
        private void clientPlayers(List<PlayerData> otherPlayers)
        {
            foreach (PlayerData player in otherPlayers)
            {
                // Create an other player sprites in this client afte
                new OtherPlayerSprite(this, player, Content.Load<Texture2D>(player.imageName),
                                        new Point(player.playerPosition.X, player.playerPosition.Y));
                connectionMessage = player.playerID + " delivered ";
            }
        }

        private void clientJoined(PlayerData otherPlayerData)
        {
            // Create an other player sprite
            new OtherPlayerSprite(this, otherPlayerData, Content.Load<Texture2D>(otherPlayerData.imageName),
                                    new Point(otherPlayerData.playerPosition.X, otherPlayerData.playerPosition.Y));
            totalPlayers.Add(otherPlayerData);
            new FadeText(this,Vector2.Zero,otherPlayerData.GamerTag + " has joined the game ");
        }

        private void clientleft(string pData)
        {
            PlayerData playerToRemove = totalPlayers.Find(l => l.playerID == pData);
        }

        private void ServerConnection_StateChanged(StateChange State)
    {
        switch (State.NewState)
        {
            case ConnectionState.Connected:
                connectionMessage = "Connected......";
                Connected = true;
                startGame();
                break;
            case ConnectionState.Disconnected:
                connectionMessage = "Disconnected.....";
                if (State.OldState == ConnectionState.Connected)
                    connectionMessage = "Lost Connection....";
                Connected = false;
                break;
            case ConnectionState.Connecting:
                connectionMessage = "Connecting.....";
                Connected = false;
                break;

        }
    }

        private void startGame()
        {
            // Continue on and subscribe to the incoming messages joined, currentPlayers, otherMove messages

            // Immediate Pattern
            proxy.Invoke<PlayerData>("Join")
                .ContinueWith( // This is an inline delegate pattern that processes the message 
                               // returned from the async Invoke Call
                        (p) => { // Wtih p do 
                            if (p.Result == null)
                                connectionMessage = "No player Data returned";
                            else
                            {
                                CreatePlayer(p.Result);
                                // Here we'll want to create our game player using the image name in the PlayerData 
                                // Player Data packet to choose the image for the player
                                // We'll use a simple sprite player for the purposes of demonstration 

                            }

                        });

                proxy.Invoke<List<Collectable>>("spawnCollectable").ContinueWith(
                   (v) =>
                   {
                       //if (v.Result == null)
                       //{
                       //    connectionMessage = "No collectable";
                       //}
                       //else
                       //{
                       //    //CreateCollectables(v.Result);
                       //}
                   });
            
            
        }

        private void CreateCollectables(List<Collectable> result)
        {
            //foreach (Collectable box in result)
            //{
            
            foreach (Collectable Item in result)
            {
                totalCollectable.Add(Item);
                DrawCollectable(Item);
            }
            
            //}

        }

        // When we get new player Data Create 
        private void CreatePlayer(PlayerData player)
        {
            Player = player;
            // Create an other player sprites in this client afte
            new SimplePlayerSprite(this, player, Content.Load<Texture2D>(player.imageName),
                                    new Point(player.playerPosition.X, player.playerPosition.Y));
            new FadeText(this, Vector2.Zero, " Welcome " + player.GamerTag + " you are playing as " + player.imageName);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService<SpriteBatch>(spriteBatch);

            font = Content.Load<SpriteFont>("Message");
            collTexture = Content.Load<Texture2D>("box");
            PlayerTex = Content.Load<Texture2D>("Player 1");
            Services.AddService<SpriteFont>(font);
            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                proxy.Invoke<string>("RemovePlayer", new object[] { Player.playerID }).ContinueWith(
                    (s) =>
                    {
                        if (s.Result == null)
                        {

                        }
                        else
                        {
                            clientleft(s.Result);
                        }
                    });

                Exit();
            }

                foreach (Collectable CollectedBox in totalCollectable)
                {
                if (CollectedBox.alive)
                {


                    Rectangle colRect = new Rectangle(CollectedBox.position.X, CollectedBox.position.Y, collTexture.Width, collTexture.Height);
                    Rectangle plaRect = new Rectangle(Player.playerPosition.X, Player.playerPosition.Y, PlayerTex.Width, PlayerTex.Height);


                    if (colRect.Intersects(plaRect))
                    {
                        
                        CollectedBox.alive = false;

                        proxy.Invoke("RemoveColl", new object[] { CollectedBox, Player });
                    }
                }
                
            }

            base.Update(gameTime);
        }

        //private void RemoveCollectable(int result)
        //{
        //    Collectable CollectableToRemove = totalCollectable.Find(l => l.id == result);
            

            
        //    //totalCollectable.Remove(CollectableToRemove);
        //}

        private void DrawCollectable(Collectable result)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);
            //SpriteBatch spriteB = Services.GetService<SpriteBatch>();
            //spriteB.Begin();
            DrawC = true;

            //spriteB.End();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            spriteBatch.Begin();

            if (DrawC == true)
            {
                foreach (Collectable CollectableToDraw in totalCollectable)
                {
                    if (CollectableToDraw.alive)
                    {


                        Rectangle colRect = new Rectangle(CollectableToDraw.position.X, CollectableToDraw.position.Y, collTexture.Width, collTexture.Height);
                        spriteBatch.Draw(collTexture, new Vector2(CollectableToDraw.position.X, CollectableToDraw.position.Y), colRect, Color.White);
                    }
                }

            }
            spriteBatch.DrawString(font, connectionMessage, new Vector2(10, 10), Color.White);
            //TODO: Add your drawing code here
            spriteBatch.End();
            base.Draw(gameTime);
        }



        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>





    }
}
