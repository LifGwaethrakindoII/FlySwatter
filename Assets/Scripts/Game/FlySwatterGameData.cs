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
    }
}