using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless.AI;

namespace Voidless.FlySwatter
{
	[CreateAssetMenu]
	public class FNNData : ScriptableObject
	{
		[SerializeField] private Bounds _bounds;
		[SerializeField] private float _gridSize;
		[SerializeField] private LayerMask _obstacleMask;
		[SerializeField, HideInInspector] private Serializable3DArray<FNNGridCell> _navigationGrid;

		/// <summary>Gets bounds property.</summary>
		public Bounds bounds { get { return _bounds; } }

		/// <summary>Gets gridSize property.</summary>
		public float gridSize { get { return _gridSize; } }

		/// <summary>Gets obstacleMask property.</summary>
		public LayerMask obstacleMask { get { return _obstacleMask; } }

		/// <summary>Gets and Sets navigationGrid property.</summary>
		public Serializable3DArray<FNNGridCell> navigationGrid
		{
			get { return _navigationGrid; }
			set { _navigationGrid = value; }
		}

		public PathFindingGrid ToPathFindingGrid()
		{
            int width = navigationGrid.GetLength(0);
            int height = navigationGrid.GetLength(1);
            int depth = navigationGrid.GetLength(2);
			PathFindingGrid graph = new PathFindingGrid(width, height, depth);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < depth; z++)
                    {
                        FNNGridCell cell = navigationGrid[x, y, z];
						PathFindingNode node = new PathFindingNode(cell.position, cell.flyable);
						graph.nodesGrid[x, y, z] = (node);
                    }
                }
            }

			graph.UpdateNeighbors();
            return graph;
        }
	}
}