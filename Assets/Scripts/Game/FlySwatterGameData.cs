using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless.FlySwatter
{
    public static class FlySwatterGameData
    {
        public const int STATE_FLAG_DEAD = 1 << 0;
        public const int STATE_FLAG_ALIVE = 1 << 1;
        public const int STATE_FLAG_WANDERING = 1 << 2;
        public const int STATE_FLAG_TOWARDSTARGET = 1 << 3;
        public const int STATE_FLAG_FLYING = 1 << 4;
        public const int STATE_FLAG_OVERSURFACE = 1 << 5;
        public const int STATE_FLAG_RESTING = 1 << 6;
        public const int STATE_FLAG_HUNGRY = 1 << 7;
        public const int STATE_FLAG_FINDINGMATE = 1 << 8;
        public const int STATE_FLAG_MATING = 1 << 9;
        public const int STATE_FLAG_PREGNANT = 1 << 10;

        public static readonly string[] FLAGNAMES_ENEMY;

        static FlySwatterGameData()
        {
            FLAGNAMES_ENEMY = new string[]
            {
                "STATE_FLAG_DEAD",
                "STATE_FLAG_ALIVE",
                "STATE_FLAG_WANDERING",
                "STATE_FLAG_TOWARDSTARGET",
                "STATE_FLAG_FLYING",
                "STATE_FLAG_OVERSURFACE",
                "STATE_FLAG_RESTING",
                "STATE_FLAG_HUNGRY",
                "STATE_FLAG_FINDINGMATE",
                "STATE_FLAG_MATING",
                "STATE_FLAG_PREGNANT"
            };
        }
    }
}