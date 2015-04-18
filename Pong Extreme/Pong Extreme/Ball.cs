using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Pong_Extreme
{
    class Ball
    {
        public Vector2 position;
        Vector2 velocity;
        Random random;
        float angle;
        Texture2D ballSprite;
        Texture2D evilSprite;
        bool isActive;
        int teamScored;
        public int idPlayer=0;
        public bool isEvil = false;

        /// <summary>
        /// Gets which team scored this ball
        /// </summary>
        public int TeamScored
        {
            get
            {
                return teamScored;
            }
        }

        /// <summary>
        /// Returns if this ball is still in play (if it wasn't scored yet)
        /// </summary>
        public bool IsActive
        {
            get
            {
                return isActive;
            }
        }

        public Ball(Texture2D newBallSprite, Texture2D newEvilSprite)
        {
            ballSprite = newBallSprite;
            evilSprite = newEvilSprite;

            isActive = true;
            random = new Random();

            angle = (float)(random.NextDouble()*Math.PI); //16f
            velocity.X = (float)(Constants.BALL_SPEED * Math.Cos(angle));
            velocity.Y = (float)(Constants.BALL_SPEED * Math.Sin(angle));
            position.X = Constants.WINDOW_WIDTH / 2;
            position.Y = (float)random.NextDouble() * Constants.WINDOW_HEIGHT / 2;
        }

        public Ball(Texture2D newBallSprite, Vector2 _position)
        {
            ballSprite = newBallSprite;

            isActive = true;
            random = new Random();

            angle = (float)(random.NextDouble() * Math.PI); //16f
            velocity.X = (float)(Constants.BALL_SPEED * Math.Cos(angle));
            velocity.Y = (float)(Constants.BALL_SPEED * Math.Sin(angle));
            position = _position;
            //position.X = Constants.WINDOW_WIDTH / 2;
            //position.Y = (float)random.NextDouble() * Constants.WINDOW_HEIGHT / 2;
        }

        public void Update(GameTime gameTime)
        {
            if (isActive)
            {
                position += velocity * gameTime.ElapsedGameTime.Milliseconds;

                //Bounce if touching top and bottom wall
                if (position.Y > Constants.WINDOW_HEIGHT - ballSprite.Height || position.Y < 0)
                {
                    velocity.Y *= -1;
                }

                if (position.X < 0)
                {
                    if(isEvil)
                        velocity.X *= -1;
                    else
                    {
                        teamScored = 2;
                        isActive = false;
                    }
                }

                if (position.X > Constants.WINDOW_WIDTH)
                {
                    if (isEvil)
                        velocity.X *= -1;
                    else
                    {
                        teamScored = 1;
                        isActive = false;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ballSprite, position, Color.White);
        }

        public void HasCollision(Vector2 otherPosition, int spriteWidth, int spriteHeight, SoundEffect bounceSound){
            if (this.position.X >= otherPosition.X - spriteWidth / 2 &&
                    this.position.X <= otherPosition.X + spriteWidth / 2 &&
                    this.position.Y >= otherPosition.Y - spriteHeight / 2 &&
                    this.position.Y <= otherPosition.Y + spriteHeight / 2)
            {
                bounceSound.Play();
                
                //see where to bounce
                float dx = otherPosition.X - this.position.X;
                float dy = otherPosition.Y - this.position.Y;

                angle = (float)Math.Atan(dy / dx);
                velocity.X = (float)(Constants.BALL_SPEED * Math.Cos(angle));
                velocity.Y = (float)(Constants.BALL_SPEED * Math.Sin(angle));

                //Correction
                if (this.position.X < otherPosition.X)
                    velocity.X *= -1;

                
                //Player touching it
                if (!this.isEvil)
                {
                    if (this.position.X < Constants.WINDOW_WIDTH / 2)
                        idPlayer = 1;
                    else
                        idPlayer = 2;
                }
            }
        }

        public void Transform()
        {
            ballSprite = evilSprite;
            idPlayer = 0;
            isEvil = true;
        }

        public void Kill(){
            isActive = false;
        }
    }
}
