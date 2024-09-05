using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Voidless.AI;
using UnityEngine.Networking;

namespace Voidless.FlySwatter
{
    public class FlyingNavigationNetworkManager : MonoBehaviour
    {
        private static readonly Color COLOR_NOTFLYABLE;
        private static readonly Color COLOR_FLYABLE;

        [SerializeField] private FNNData _networkData;
        [SerializeField, HideInInspector] private PathFindingGrid grid;
        private AStarPathFindingAlgorithm pathfinding;
        private IPathFindingNode<Vector3> start;
        private IPathFindingNode<Vector3> end;
        private List<IPathFindingNode<Vector3>> path;

        /// <summary>Gets networkData property.</summary>
        public FNNData networkData { get { return _networkData; } }

        static FlyingNavigationNetworkManager()
        {
            COLOR_NOTFLYABLE = Color.red;
            COLOR_NOTFLYABLE.a = 0.35f;
            COLOR_FLYABLE = Color.white;
            COLOR_FLYABLE.a = 0.01f;
        }

        /// <summary>Draws Gizmos on Editor mode when FlyingNavigationNetworkManager's instance is selected.</summary>
        private void OnDrawGizmosSelected()
        {
            if(networkData == null) return;
            
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(networkData.bounds.center, networkData.bounds.size);
            
            if(networkData.navigationGrid == null) return;

            float gridSize = networkData.gridSize;
            Vector3 gridDimensions = Vector3.one * gridSize;

            foreach(FNNGridCell cell in networkData.navigationGrid)
            {
                Vector3 position = cell.position;

                switch(cell.flyable)
                {
                    case true:
                        Gizmos.color = COLOR_FLYABLE;
                        Gizmos.DrawWireCube(position, gridDimensions);
                    break;

                    case false:
                        Gizmos.color = COLOR_NOTFLYABLE;
                        Gizmos.DrawCube(position, gridDimensions);
                    break;
                }
            }

            if(grid == null || grid.nodes == null) return;

            Gizmos.color = VColor.transparentWhite;
            int i = 0;

            foreach(PathFindingNode node in grid)
            {
                VGizmos.DrawText(i.ToString(), node.data, Vector2.zero, Color.white);
                Gizmos.DrawSphere(node.data, 0.05f);
                i++;
            }

            Gizmos.color = Color.magenta;
            if(start != null) Gizmos.DrawSphere(start.data, 0.2f);
            Gizmos.color = Color.cyan;
            if(end != null) Gizmos.DrawSphere(end.data, 0.2f);

            if(path == null) return;

            Vector3? p = null;

            foreach(IPathFindingNode<Vector3> node in path)
            {
                Gizmos.DrawSphere(node.data, 0.1f);
                if(p.HasValue) Gizmos.DrawLine(p.Value, node.data);
                p = node.data;
            }
        }

        [Button("Bake Network")]
        private void BakeNetwork()
        {
            if(networkData == null) return;

            Vector3 size = networkData.bounds.size;
            Vector3 min = networkData.bounds.min;
            float gridSize = networkData.gridSize;
            int width = Mathf.RoundToInt(size.x / gridSize);
            int height = Mathf.RoundToInt(size.y / gridSize);
            int depth = Mathf.RoundToInt(size.z / gridSize);

            networkData.navigationGrid = new Serializable3DArray<FNNGridCell>(width, height, depth);

            for(int x = 0; x < width; x++)
            {
                for(int y = 0; y < height; y++)
                {
                    for(int z = 0; z < depth; z++)
                    {
                        Vector3 position = min + (new Vector3(x, y, z) * gridSize);
                        bool flyable = !Physics.CheckSphere(position, gridSize * 0.5f, networkData.obstacleMask);
                        networkData.navigationGrid[x, y, z] = new FNNGridCell(position, flyable);
                    }
                }
            }
        }

        [Button("Test Grid")]
        private void CreateGrid()
        {
            grid = networkData.ToPathFindingGrid();
        }

        [Button("Test Pathfinding")]
        private void TEST_Pathfinding(Vector3 from, Vector3 to)
        {
            pathfinding = new AStarPathFindingAlgorithm();
            start = grid.GetClosestNode(from);
            end = grid.GetClosestNode(to);
            path = pathfinding.CalculatePath(start, end);

            Debug.Log("[FlyingNavigationNetworkManager] Calculated Path Successfully? " + (path != null));
        }

        public Vector3Int GetGridIndices(Vector3 position)
        {
            if(networkData == null) return default;

            Vector3 localPosition = position - networkData.bounds.min;
            float gridSize = networkData.gridSize;

            return new Vector3Int
            (
                Mathf.FloorToInt(localPosition.x / gridSize),
                Mathf.FloorToInt(localPosition.y / gridSize),
                Mathf.FloorToInt(localPosition.z / gridSize)
            );
        }
    }
}