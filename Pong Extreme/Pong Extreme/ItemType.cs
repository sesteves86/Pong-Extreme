using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pong_Extreme
{
    public enum ItemType
    {
        //Ball Related
        DuplicateBall=0, //D
        TransformIntoDragon=1, //T

        //Player Related
        IncrementSize=2, //S+
        DecrementSize=3, //S-
        IncreasePlayerVelocity=4, //V+
        DecreasePlayerVelocity=5, //V-
        IncrementTail=6, //T+
        DecrementTail=7 //T-
    };

    public enum Menus
    {
        StartUp,
        Game,
        Aftermatch
    };
}
