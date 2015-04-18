using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Pong_Extreme
{
    class Player
    {
        public Texture2D playerSprite;
        public Vector2 position;
        Vector2 velocity;
        int id;
        int score;
        int oldScore;
        public int tailSize = 0;
        public List<BodyPlayer> bodyList;
        Vector2 desiredBodyVelocity;
        Rectangle drawRectangle;
        public int sizeModifier = 0;
        public int velocityModifier = 0;
        Random random = new Random();
        //SoundEffect bounceSound;

        public int Score
        {
            get
            {
                return score;
            }
            set
            {
                score = value;
            }
        }

        public Player(int _id, Texture2D _playerSprite)
        {
            id = _id;
            playerSprite = _playerSprite;

            bodyList = new List<BodyPlayer>(50);

            score = 0;
            oldScore = 0;
            position.Y = Constants.WINDOW_HEIGHT / 2;
            position.X = Constants.WINDOW_WIDTH / 8;
            if (id == 2)
            {
                position.X += Constants.WINDOW_WIDTH * 6 / 8;
            }
        }

        public void Update(GameTime gameTime)
        {
            //Update position
            if(velocityModifier<1){velocityModifier=1;}

            position += velocity * gameTime.ElapsedGameTime.Milliseconds * (5+velocityModifier);

            //Read keys
            switch (id)
            {
                case 1:
                    ReadKeys1();
                    break;
                case 2:
                    ReadKeys2();
                    break;
            }

            //Keep players inside their boxes
            if (position.Y < Constants.BOX_TOP)
            {
                position.Y = Constants.BOX_TOP;
                velocity.Y = 0;
            } else if(position.Y>Constants.BOX_BOTTOM){
                position.Y = Constants.BOX_BOTTOM;
                velocity.Y = 0;
            }

            if (position.X < id * 600 - 570)
            {
                position.X = id * 600 - 570;
                velocity.X = 0;
            }
            else if (position.X > id * 600 - 460)
            {
                position.X = id * 600 - 460;
                velocity.X = 0;
            }

            //Update body parts
            for (int i = 0 ; i < bodyList.Count; i++)
            {
                if (i==0){
                    desiredBodyVelocity.X = position.X - bodyList[i].GetPosition.X ;
                    desiredBodyVelocity.Y = position.Y - bodyList[i].GetPosition.Y;
                } else {
                    desiredBodyVelocity.X = bodyList[i-1].GetPosition.X - bodyList[i].GetPosition.X;
                    desiredBodyVelocity.Y = bodyList[i-1].GetPosition.Y - bodyList[i].GetPosition.Y;
                }

                bodyList[i].Update(gameTime, desiredBodyVelocity);
            }

            //Check if there was a goal
            if (score > oldScore)
            {
                oldScore++;
                //tailSize;
                Vector2 newBodyPosition;

                if (tailSize==0)
                    newBodyPosition = new Vector2(position.X, position.Y);
                else
                    newBodyPosition = new Vector2(bodyList[tailSize-1].GetPosition.X, bodyList[tailSize-1].GetPosition.Y);
                
                bodyList.Add(new BodyPlayer(newBodyPosition, playerSprite));

                tailSize = bodyList.Count;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
            foreach (var body in bodyList)
            {
                drawRectangle = new Rectangle((int)body.GetPosition.X, (int)body.GetPosition.Y, Constants.PLAYER_SIZE + sizeModifier, Constants.PLAYER_SIZE + sizeModifier);
                spriteBatch.Draw(playerSprite, drawRectangle, Color.Gray);
            }
            drawRectangle = new Rectangle((int)position.X, (int)position.Y, Constants.PLAYER_SIZE + sizeModifier, Constants.PLAYER_SIZE + sizeModifier);
            spriteBatch.Draw(playerSprite, drawRectangle, Color.White);
        }

        public void IncrementTailSize()
        {
            tailSize++;
            //bodyList.Add(new BodyPlayer(newBodyPosition, playerSprite));
        }

        public void DecrementTailSize()
        {
            tailSize--;
        }

        void ReadKeys1()
        {
            var keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Keys.W))
                velocity.Y -= Constants.PLAYER_SPEED_MODIFIER;
            else if (keyboard.IsKeyDown(Keys.S))
                velocity.Y += Constants.PLAYER_SPEED_MODIFIER;
            else
                velocity.Y /= Constants.PLAYER_SPEED_DECAY;

            if (keyboard.IsKeyDown(Keys.A))
                velocity.X -= Constants.PLAYER_SPEED_MODIFIER;
            else if (keyboard.IsKeyDown(Keys.D))
                velocity.X += Constants.PLAYER_SPEED_MODIFIER;
            else
                velocity.X /= Constants.PLAYER_SPEED_DECAY;
        }

        void ReadKeys2()
        {
            var keyboard = Keyboard.GetState();
            
            if (keyboard.IsKeyDown(Keys.Up))
                velocity.Y -= Constants.PLAYER_SPEED_MODIFIER;
            else if (keyboard.IsKeyDown(Keys.Down))
                velocity.Y += Constants.PLAYER_SPEED_MODIFIER;
            else
                velocity.Y /= Constants.PLAYER_SPEED_DECAY;

            if (keyboard.IsKeyDown(Keys.Left))
                velocity.X -= Constants.PLAYER_SPEED_MODIFIER;
            else if (keyboard.IsKeyDown(Keys.Right))
                velocity.X += Constants.PLAYER_SPEED_MODIFIER;
            else
                velocity.X /= Constants.PLAYER_SPEED_DECAY;
        }

        internal void TouchedByEvil()
        {
            tailSize /=2 ;
            if (tailSize < 0)
                tailSize = 0;
            for (int i = bodyList.Count-1; i >= tailSize; i--)
            {
                bodyList.RemoveAt(i);
            }
        }
    }
}
