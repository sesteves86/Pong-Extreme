using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pong_Extreme
{
    static class Constants
    {
        //Velocities
        public const float BALL_SPEED = 0.2f;
        public const float PLAYER_SPEED_MODIFIER = 0.01f;
        public const float PLAYER_BODY_VELOCITY_MODIFIER = 0.01f;
        public const float PLAYER_SPEED_DECAY = 1.1f;

        //Timers
        public const int NEW_BALL_TIMER = 5000;
        public const int ITEM_TIMER_TRIGGER = 3000;//10000;
        public const int MAX_SCORE_EFFECTS_TIME = 200;

        //Dimensions
        public const int WINDOW_WIDTH = 800;
        public const int WINDOW_HEIGHT = 600;
        public const int ITEM_SIZE = 30;
        public const int PLAYER_SIZE = 30;
        public const int BALL_SIZE = 30;
        public const int SCORE_TREMBLE = 5;
        public const int BOX_TOP = 20;
        public const int BOX_BOTTOM = 550;
        public const int BOX_LEFT = 20;
        public const int BOX_RIGHT = 20;
    }
}
