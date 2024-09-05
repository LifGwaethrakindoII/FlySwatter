using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*===========================================================================
**
** Class:  AStarNode
**
** Purpose: Node structure for A* implementation.
**
**
** Author: Lîf Gwaethrakindo
**
===========================================================================*/
namespace Voidless
{
    public class AStarNode
    {
        public const float COST_G_NORMAL = 10.0f;
        public const float COST_G_DIAGONAL = 14.0f;

        [SerializeField] private AStarNode _parent;
        [SerializeField] private Vector3 _position;
        [SerializeField] private Vector3Int _gridPosition;
        [SerializeField] private float _gCost;
        [SerializeField] private float _hCost;
        [SerializeField] private bool _walkable;

        /// <summary>Gets and Sets parent property.</summary>
        public AStarNode parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        /// <summary>Gets and Sets position property.</summary>
        public Vector3 position
        {
            get { return _position; }
            set { _position = value; }
        }

        /// <summary>Gets and Sets gridPosition property.</summary>
        public Vector3Int gridPosition
        {
            get { return _gridPosition; }
            set { _gridPosition = value; }
        }

        /// <summary>Gets and Sets gCost property.</summary>
        public float gCost
        {
            get { return _gCost; }
            set { _gCost = value; }
        }

        /// <summary>Gets and Sets hCost property.</summary>
        public float hCost
        {
            get { return _hCost; }
            set { _hCost = value; }
        }

        /// <summary>Gets and Sets walkable property.</summary>
        public bool walkable
        {
            get { return _walkable; }
            set { _walkable = value; }
        }

        /// <summary>Gets fCost property.</summary>
        public float fCost { get { return gCost + hCost; } }
    
        /// <summary>AStarNode's constructor.</summary>
        public AStarNode(Vector3Int _gridPosition)
        {
            gridPosition = _gridPosition;
            gCost = float.MaxValue;
            hCost = 0.0f;
            parent = null;
        }

        /// <summary>AStarNode's constructor.</summary>
        public AStarNode(Vector3 _position)
        {
            position = _position;
            gCost = float.MaxValue;
            hCost = 0.0f;
            parent = null;
        }
    }
}