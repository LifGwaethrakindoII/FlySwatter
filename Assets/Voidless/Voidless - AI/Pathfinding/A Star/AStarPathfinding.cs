using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
    public class AStarPathfinding
    {
        private float movementCostScalar = 1.0f;

        public List<AStarNode> FindPath(AStarNode start, AStarNode end, Func<AStarNode, List<AStarNode>> getNeighbors, Func<AStarNode, AStarNode, float> h)
        {
            List<AStarNode> open = new List<AStarNode>();
            HashSet<AStarNode> closed = new HashSet<AStarNode>();

            open.Add(start);

            while(open.Count > 0)
            {
                AStarNode current = open[0];

                for(int i = 1; i < open.Count; i++)
                {
                    AStarNode node = open[i];
                    if(node.fCost < current.fCost || node.fCost == current.fCost && node.hCost < current.hCost)
                    current = node;
                }

                open.Remove(current);
                closed.Add(current);

                if(current == end) return RetracePath(start, end);

                foreach(AStarNode neighbor in getNeighbors(current))
                {
                    if(closed.Contains(neighbor)) continue;

                    float newMovementCostToNeighbor = current.gCost + h(current, neighbor) * movementCostScalar;
                    if (newMovementCostToNeighbor < neighbor.gCost || !open.Contains(neighbor))
                    {
                        neighbor.gCost = newMovementCostToNeighbor;
                        neighbor.hCost = h(neighbor, end);
                        neighbor.parent = current;

                        if(!open.Contains(neighbor))
                        {
                            Debug.Log("[AStarPathfinding] Adding to open with position: " + neighbor.position);
                            open.Add(neighbor);
                        }
                    }
                }
            }

            return null;
        }

        private List<AStarNode> RetracePath(AStarNode start, AStarNode end)
        {
            List<AStarNode> path = new List<AStarNode>();
            AStarNode current = end;

            while (current != start)
            {
                path.Add(current);
                current = current.parent;
            }

            path.Reverse();

            foreach(AStarNode node in path)
            {
                Debug.Log("[AStarPathfinding] Node with position " + node.position);
            }

            return path;
        }

        /// <summary>Calculates the Manhattan distance (sum of the component-wise differences) of 2 vectors.</summary>
        /// <param name="nodeA">AStarNode A.</param>
        /// <param name="nodeB">AStarNode B.</param>
        /// <returns>Manhattan distance between AStarNodes A and B.</returns>
        public static float ManhattanDistance(AStarNode nodeA, AStarNode nodeB)
        {
            Vector3 a = nodeA.position;
            Vector3 b = nodeB.position;

            return VVector3.ManhattanDistance(a, b);
        }

        /// <summary>Calculates the Chebysher distance (maximum of the component-wise differences) of 2 vectors.</summary>
        /// <param name="nodeA">AStarNode A.</param>
        /// <param name="nodeB">AStarNode B.</param>
        /// <returns>Chebysher distance between AStarNodes A and B.</returns>
        public static float ChebysherDistance(AStarNode nodeA, AStarNode nodeB)
        {
            Vector3 a = nodeA.position;
            Vector3 b = nodeB.position;
            
            return VVector3.ChebysherDistance(a, b);
        }
    }
}