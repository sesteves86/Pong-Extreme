using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pong_Extreme
{
    class BodyPlayer
    {
        Vector2 position;
        Vector2 velocity;
        Texture2D ballSprite;

        public Vector2 GetPosition
        {
            get
            {
                return position;
            }
        }

        public BodyPlayer(Vector2 newPosition, Texture2D newBallSprite)
        {
            position = newPosition;
            ballSprite = newBallSprite;

            velocity = new Vector2();
        }

        public void Update(GameTime gameTime, Vector2 dist)
        {
            //float atr = dist.X * Constants.PLAYER_BODY_VELOCITY_MODIFIER;
            //float rep = 1 / dist.X;
            //if (dist.X == 0)
            //    dist.X = 0.0001f;
            //if (dist.Y == 0)
            //    dist.Y = 0.0001f;

            //velocity.X += (dist.X * Constants.PLAYER_BODY_VELOCITY_MODIFIER) - (1 / dist.X);
            //velocity.Y += (dist.Y * Constants.PLAYER_BODY_VELOCITY_MODIFIER) - (1 / dist.Y);
            velocity += dist * Constants.PLAYER_BODY_VELOCITY_MODIFIER;
            position = velocity * gameTime.ElapsedGameTime.Milliseconds;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ballSprite, position, Color.White);
        }
    }
}
