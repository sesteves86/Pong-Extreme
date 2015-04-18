using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Pong_Extreme
{
    class Score
    {
        Vector2 position1;
        Vector2 position2;
        Random random;

        int score1 = 0;
        int oldScore1 = 0;
        int timer1 = 0;

        int score2 = 0;
        int oldScore2 = 0;
        int timer2=0;

        Color color = new Color(120, 120, 120, 0.01f);

        public Score()
        {
            position1.X = 50;
            position1.Y = 10;
            position2.X = 750;
            position2.Y = 10;

            random = new Random();
        }

        //private void Update()

        public void Draw(SpriteBatch spriteBatch, SpriteFont font, int _score1, int _score2)
        {
            timer1++;
            timer2++;

            //Score 1
            if (_score1 != oldScore1)
            {
                timer1 = 0;
                oldScore1 = _score1;
            }
            spriteBatch.DrawString(font, _score1.ToString(), new Vector2(position1.X, position1.Y), Color.White);
            int numSprites1 = (int)(Constants.MAX_SCORE_EFFECTS_TIME - timer1)/50;
            for (int i=0; i<(numSprites1); i++){
                int posRandomX = random.Next(-Constants.SCORE_TREMBLE, Constants.SCORE_TREMBLE);
                int posRandomY = random.Next(-Constants.SCORE_TREMBLE, Constants.SCORE_TREMBLE);
                spriteBatch.DrawString(font, _score1.ToString(), new Vector2(position1.X+posRandomX,position1.Y+posRandomY), color);
            }

            //Score 2
            if (_score2 != oldScore2)
            {
                timer2 = 0;
                oldScore2 = _score2;
            }
            spriteBatch.DrawString(font, _score2.ToString(), new Vector2(position2.X, position2.Y), Color.White);
            int numSprites2 = (int)(Constants.MAX_SCORE_EFFECTS_TIME - timer2) / 50;
            for (int i = 0; i < (numSprites2); i++)
            {
                int posRandomX = random.Next(-Constants.SCORE_TREMBLE, Constants.SCORE_TREMBLE);
                int posRandomY = random.Next(-Constants.SCORE_TREMBLE, Constants.SCORE_TREMBLE);
                spriteBatch.DrawString(font, _score2.ToString(), new Vector2(position2.X + posRandomX, position2.Y + posRandomY), color);
            }
        }
    }
}
