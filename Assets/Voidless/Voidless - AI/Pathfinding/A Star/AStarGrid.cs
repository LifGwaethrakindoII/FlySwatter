using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.XR.Interaction.Toolkit.Inputs.Interactions.SectorInteraction;

namespace Voidless
{
    [Serializable]
    public class AStarGrid
    {
        public static readonly Vector3Int[] DIRECTIONS_NEIGHBOR;

        private AStarNode[,,] _nodes;
        private int _width, _height, _depth;

        /// <summary>Gets and Sets width property.</summary>
        public int width
        {
            get { return _width; }
            set { _width = value; }
        }

        /// <summary>Gets and Sets height property.</summary>
        public int height
        {
            get { return _height; }
            set { _height = value; }
        }

        /// <summary>Gets and Sets depth property.</summary>
        public int depth
        {
            get { return _depth; }
            set { _depth = value; }
        }

        /// <summary>Gets and Sets nodes property.</summary>
        public AStarNode[,,] nodes
        {
            get { return _nodes; }
            set { _nodes = value; }
        }

        /// <summary>Gets and Sets AStarNode from array of AStarNodes.</summary>
        public AStarNode this[int x, int y, int z]
        {
            get { return GetNode(x, y, z); }
            set { if(IsValidPosition(x, y, z)) nodes[x, y, z] = value; }
        }

        static AStarGrid()
        {
            DIRECTIONS_NEIGHBOR = new Vector3Int[]
            {
                Vector3Int.left,
                Vector3Int.right,
                Vector3Int.down,
                Vector3Int.up,
                Vector3Int.back,
                Vector3Int.forward
            };
        }

        /// <summary>Gets and Sets AStarNode from array of AStarNodes.</summary>
        public AStarNode this[Vector3Int i]
        {
            get { return GetNode(i); }
            set { this[i.x, i.y, i.z] = value; }
        }

        /// <summary>AStarGrid's constructor.</summary>
        /// <param name="_width">Dimension in the X-Axis.</param>
        /// <param name="_height">Dimension in the Y-Axis.</param>
        /// <param name="_depth">Dimension in the Z-Axis.</param>
        public AStarGrid(int _width, int _height, int _depth)
        {
            width = _width;
            height = _height;
            depth = _depth;
            nodes = new AStarNode[width, height, depth];
        }

        /// <summary>Evaluates whether a set of indices is valid.</summary>
        /// <param name="x">X's index.</param>
        /// <param name="y">Y's index.</param>
        /// <param name="z">Z's index.</param>
        /// <returns>True if the indices are within the dimension bounds.</returns>
        public bool IsValidPosition(int x, int y, int z)
        {
            return (x >= 0 && x < width)
            && (y >= 0 && y < height)
            && (z >= 0 && z < depth);
        }

        /// <summary>Evaluates whether a set of indices is valid.</summary>
        /// <param name="i">Set of indices as a Vector3Int.</param>
        /// <returns>True if the indices are within the dimension bounds.</returns>
        public bool IsValidPosition(Vector3Int i)
        {
            return IsValidPosition(i.x, i.y, i.z);
        }

        /// <summary>Returns Node given a set of indices.</summary>
        /// <param name="x">X's index.</param>
        /// <param name="y">Y's index.</param>
        /// <param name="z">Z's index.</param>
        public AStarNode GetNode(int x, int y, int z)
        {
            return IsValidPosition(x, y, z) ? nodes[x, y, z] : null;
        }

        /// <summary>Returns Node given a set of indices.</summary>
        /// <param name="i">Set of indices as a Vector3Int.</param>
        public AStarNode GetNode(Vector3Int i)
        {
            return GetNode(i.x, i.y, i.z);
        }

        public List<AStarNode> GetNeighbors(AStarNode node)
        {
            return GetNeighbors(node.gridPosition);
        }

        public List<AStarNode> GetNeighbors(Vector3Int i)
        {
            List<AStarNode> neighbors = new List<AStarNode>();

            foreach (var direction in DIRECTIONS_NEIGHBOR)
            {
                int newX = i.x + direction.x;
                int newY = i.y + direction.y;
                int newZ = i.z + direction.z;
                AStarNode neighbor = GetNode(newX, newY, newZ);

                // Check if the new position is within grid bounds and is walkable
                if (IsValidPosition(newX, newY, newZ) && neighbor != null && neighbor.walkable)
                neighbors.Add(neighbor);
            }

            return neighbors;
        }

        /// <returns>True if Neighbor's set of indices are of that of a diagonal neighbor.</returns>
        public bool DiagonalNeighbor(Vector3Int i)
        { /// This basically says: "if it takes more than 1 step to reach neighbor, it is diagonal".
            return i.Sum() > 1;
        }
    }
}