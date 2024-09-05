using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Voidless.AI
{
    public class AStarPathFindingAlgorithm : IPathFindingAlgorithm<Vector3>
    {
        public List<IPathFindingNode<Vector3>> CalculatePath(IPathFindingNode<Vector3> start, IPathFindingNode<Vector3> end)
        {
            if(start == null || end == null) return null;

            List<IPathFindingNode<Vector3>> open = new List<IPathFindingNode<Vector3>>();  // Priority Queue
            HashSet<IPathFindingNode<Vector3>> closed = new HashSet<IPathFindingNode<Vector3>>();

            open.Add(start);

            while (open.Count > 0)
            {
                // Get node with lowest FCost()
                IPathFindingNode<Vector3> currentNode = open.OrderBy(n => n.FCost()).ThenBy(n => n.hCost).First();

                if (currentNode == end)
                {
                    // Path found, reconstruct the path
                    return ReconstructPath(currentNode);
                }

                open.Remove(currentNode);
                closed.Add(currentNode);

                foreach (var neighbor in currentNode.neighbors)
                {
                    if (!neighbor.traversable || closed.Contains(neighbor))
                    {
                        continue;
                    }

                    float tentativegCost = currentNode.gCost + Vector3.Distance(currentNode.data, neighbor.data);

                    if (tentativegCost < neighbor.gCost || !open.Contains(neighbor))
                    {
                        neighbor.gCost = tentativegCost;
                        neighbor.hCost = Vector3.Distance(neighbor.data, end.data);
                        neighbor.parent = currentNode;

                        if (!open.Contains(neighbor))
                        {
                            open.Add(neighbor);
                        }
                    }
                }
            }

            return null; // No path found
        }

        private List<IPathFindingNode<Vector3>> ReconstructPath(IPathFindingNode<Vector3> node)
        {
            List<IPathFindingNode<Vector3>> path = new List<IPathFindingNode<Vector3>>();
            IPathFindingNode<Vector3> currentNode = node;

            while (currentNode != null)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }

            path.Reverse();
            return path;
        }
    }
}