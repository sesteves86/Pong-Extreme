using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Pong_Extreme
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D backgroundSprite;
        Texture2D startupBackground;
        Texture2D player1Sprite;
        Texture2D player2Sprite;
        Texture2D ballSprite;
        Texture2D evilBallSprite;
        Texture2D itemStripeSprite;
        SpriteFont font;

        SoundEffect bounceSound;
        SoundEffect evilSound;

        Score score;

        Player player1;
        Player player2;
        List<Ball> ballList;
        List<Items> itemList;

        Menus menu;

        Random random;
        int timeElapsedBalls = 0;
        int timeElapsedItems = 0;

        bool isToAddBall = false;
        int winner = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = Constants.WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = Constants.WINDOW_HEIGHT;

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
            random = new Random();
            menu = Menus.StartUp;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            backgroundSprite = Content.Load<Texture2D>("Pong Background");
            startupBackground = Content.Load<Texture2D>("Startup Menu");
            player1Sprite = Content.Load<Texture2D>("Small Red Ball");
            player2Sprite = Content.Load<Texture2D>("Small Yellow Ball");
            ballSprite = Content.Load<Texture2D>("Brown Ball");
            evilBallSprite = Content.Load<Texture2D>("Evil Ball");
            itemStripeSprite = Content.Load<Texture2D>("ItemStrip");
            font = Content.Load<SpriteFont>("SpriteFont1");

            bounceSound = Content.Load<SoundEffect>("Bounce");
            evilSound = Content.Load<SoundEffect>("Muhaha");

            //Initialize all items
            player1 = new Player(1, player1Sprite);
            player2 = new Player(2, player2Sprite);
            ballList = new List<Ball>();
            ballList.Add(new Ball(ballSprite, evilBallSprite));
            itemList = new List<Items>();
            score = new Score();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            switch(menu)
            {
                case Menus.StartUp:
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                        menu = Menus.Game;
                    break;
                case Menus.Aftermatch:

                    break;
                case Menus.Game:
                    #region Timers
                    //Update timers ==> check if needed to put more items on the game
                    timeElapsedBalls += gameTime.ElapsedGameTime.Milliseconds;
                    if (timeElapsedBalls > Constants.NEW_BALL_TIMER)
                    {
                        ballList.Add(new Ball(ballSprite, evilBallSprite));
                        timeElapsedBalls = 0;
                    }

                    timeElapsedItems += gameTime.ElapsedGameTime.Milliseconds;
                    if (timeElapsedItems > Constants.ITEM_TIMER_TRIGGER)
                    {
                        Vector2 tempPosition;
                        tempPosition.X = (float)(random.NextDouble()*400+200);
                        tempPosition.Y = (float)(random.NextDouble()*400+100);
                        int nItemType = random.Next(7)+1;
                        ItemType itemType = GetItemType(nItemType);
                        itemList.Add(new Items(itemStripeSprite,tempPosition,itemType, nItemType));
                        timeElapsedItems=0;
                    }
                    #endregion

                    player1.Update(gameTime);
                    player2.Update(gameTime);

                    #region foreach ball
                    foreach (Ball ball in ballList)
                    {
                        ball.Update(gameTime);
                    }

                    foreach(Ball ball in ballList)
                    {
                        //Check impact against each player
                        ball.HasCollision(player1.position, player1.playerSprite.Width, player1.playerSprite.Height, bounceSound);
                        ball.HasCollision(player2.position, player2.playerSprite.Width, player2.playerSprite.Height, bounceSound);
                
                        //Check impact against each player's tail
                        foreach (var body in player1.bodyList)
                        {
                            ball.HasCollision(body.GetPosition, player1.playerSprite.Width, player1.playerSprite.Height, bounceSound);
                        }
                        foreach (var body in player2.bodyList)
                        {
                            ball.HasCollision(body.GetPosition, player2.playerSprite.Width, player2.playerSprite.Height, bounceSound);
                        }

                        //Check impact against other balls
                        foreach (var otherBall in ballList)
                        {
                            if (otherBall != ball)
                            {
                                ball.HasCollision(otherBall.position, player1.playerSprite.Width, player1.playerSprite.Height, bounceSound);
                            }
                        }

                        //Check impact with item
                        for (int i=0; i<itemList.Count; i++)
                        {
                            if (itemList[i].hasCollision(ball.position))
                            {
                                ItemType itemType = itemList[i].itemType;
                                int playerID = ball.idPlayer;
                                //Do whatever the itemType does
                                ProcessItemCrash(itemType, ball, playerID, evilSound);
                                itemList.RemoveAt(i);
                            }
                        }

                        if (ball.isEvil)
                        {
                            if (ball.idPlayer == 1)
                            {
                                player1.TouchedByEvil();
                                ball.Kill();
                            } else if (ball.idPlayer == 2)
                            {
                                player2.TouchedByEvil();
                                ball.Kill();
                            }
                    
                        }
            
                    }
                    #endregion

                    if (isToAddBall)
                    {
                        isToAddBall = false;
                        ballList.Add(new Ball(ballSprite, evilBallSprite));
                    }

                    //Check if there's score
                    for (int i = 0; i < ballList.Count; i++)
                    {
                        if (!ballList[i].IsActive)
                        {
                            int scorer = ballList[i].TeamScored;
                            if (scorer == 1) { player1.Score = player1.Score + 1; }
                            else { player2.Score = player2.Score + 1; }
                            ballList.RemoveAt(i);
                        }
                    }

                    //Check if game is over
                    if(player1.Score>=10 || player2.Score >=10){
                        menu = Menus.Aftermatch;
                        if (player1.Score > player2.Score)
                            winner = 1;
                        else
                            winner = 2;
                    }
                    break;
                
            }

            base.Update(gameTime);
        }

        private void ProcessItemCrash(ItemType itemType, Ball ball, int idPlayer, SoundEffect evilSound)
        {
            Player player;

            if (idPlayer == 1)
                player = player1;
            else
                player = player2;


            switch (itemType)
            {
                case ItemType.DuplicateBall:
                    isToAddBall = true;
                    break;
                case ItemType.DecrementSize:
                    player.sizeModifier -= 5;
                    break;
                case ItemType.IncrementSize:
                    player.sizeModifier += 5;
                    break;
                case ItemType.DecreasePlayerVelocity:
                    player.velocityModifier -= 1;
                    break;
                case ItemType.IncreasePlayerVelocity:
                    player.velocityModifier += 1;
                    break;
                case ItemType.TransformIntoDragon:
                    evilSound.Play();
                    ball.Transform();
                    break;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            spriteBatch.Begin();

            switch (menu)
            {
                case Menus.StartUp:
                    spriteBatch.Draw(startupBackground, new Rectangle(0,0,Constants.WINDOW_WIDTH, Constants.WINDOW_HEIGHT), Color.White);
                    break;
                case Menus.Aftermatch:
                    spriteBatch.DrawString(font, "Player " + winner.ToString() + " wins!", new Vector2(300, 100), Color.Black);
                    spriteBatch.DrawString(font, "Player1 score: \n" + player1.Score.ToString(), new Vector2(100, 300), Color.Red);
                    spriteBatch.DrawString(font, "Player2 score: \n" + player2.Score.ToString(), new Vector2(600, 300), Color.Red);
                    break;
                case Menus.Game:

                    spriteBatch.Draw(backgroundSprite, new Vector2(), Color.White);

                    player1.Draw(spriteBatch);
                    player2.Draw(spriteBatch);

                    foreach (Ball ball in ballList)
                    {
                        ball.Draw(spriteBatch);
                    }

                    foreach (Items item in itemList)
                    {
                        item.Draw(spriteBatch);
                    }

                    //Draw Score
                    score.Draw(spriteBatch, font, player1.Score, player2.Score);

                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }


        public ItemType GetItemType(int id)
        {
            ItemType itemType;
            switch (id)
            {
                case 0:
                    itemType = ItemType.DuplicateBall;
                    break;
                case 1:
                    itemType = ItemType.TransformIntoDragon;
                    break;
                case 2:
                    itemType = ItemType.IncrementSize;
                    break;
                case 3:
                    itemType = ItemType.DecrementSize;
                    break;
                case 4:
                    itemType = ItemType.IncreasePlayerVelocity;
                    break;
                case 5:
                    itemType = ItemType.DecreasePlayerVelocity;
                    break;
                default:
                    itemType = ItemType.DuplicateBall;
                    break;
            }
            return itemType;
        }
    }

}
