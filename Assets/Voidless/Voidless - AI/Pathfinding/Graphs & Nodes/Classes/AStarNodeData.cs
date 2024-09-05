using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless.AI
{
    [Serializable]
    public struct AStarNodeData
    {
        public Vector3 position;
        public float gCost;
        public float hCost;
        public int flags;

        public float fCost { get { return gCost + hCost; } }
    }
}