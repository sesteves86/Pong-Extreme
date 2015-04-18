using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pong_Extreme
{
    class Items
    {
        Texture2D itemStripSprite;
        //Texture2D itemSprite;
        Rectangle sourceRectangle;
        Vector2 position;
        //Random random;
        public ItemType itemType;
        Rectangle drawRectangle;

        public Items(Texture2D _itemSprite, Vector2 _position, ItemType _itemType, int idItemType)
        {
            itemStripSprite = _itemSprite;
            itemType = _itemType;
            position = _position;

            //Get the correct sprite
            sourceRectangle = new Rectangle(idItemType * 50, 0, 50, 50);
            //sourceRectangle.X = idItemType*50;
            //itemSprite = 1;
        }

        public void Update(GameTime gameTime)
        {
            //If in contact with a ball, then die and grant bonus
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(itemSprite, position, Color.White);
            drawRectangle = new Rectangle((int)position.X, (int)position.Y, Constants.ITEM_SIZE, Constants.ITEM_SIZE);
            spriteBatch.Draw(itemStripSprite, drawRectangle, sourceRectangle, Color.White);
        }

        internal bool hasCollision(Vector2 ballPosition)
        {
            if (this.position.X >= ballPosition.X - Constants.BALL_SIZE / 2 &&
                    this.position.X <= ballPosition.X + Constants.BALL_SIZE / 2 &&
                    this.position.Y >= ballPosition.Y - Constants.BALL_SIZE / 2 &&
                    this.position.Y <= ballPosition.Y + Constants.BALL_SIZE / 2)
            {
                return true;
            }
            else { return false; }

        }
    }
}
