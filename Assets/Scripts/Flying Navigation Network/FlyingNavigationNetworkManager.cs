using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Voidless.AI;
using Voidless.AI.PathFinding;
using UnityEngine.Networking;

namespace Voidless.FlySwatter
{
    public class FlyingNavigationNetworkManager : MonoBehaviour
    {
        private static readonly Color COLOR_NOTFLYABLE;
        private static readonly Color COLOR_FLYABLE;

        [SerializeField] private FNNData _networkData;
        private AStarPathFindingAlgorithm pathfinding;
        private IPFNode<Vector3> start;
        private IPFNode<Vector3> end;
        private List<IPFNode<Vector3>> path;
        private PFOctaTreeGrid treeGrid;

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
            if(treeGrid != null)
            {
                treeGrid.DrawGizmos();
                Gizmos.DrawCube(Vector3.one, Vector3.one);
            }

            Gizmos.color = Color.magenta;
            if(start != null) Gizmos.DrawSphere(start.data, 0.2f);
            Gizmos.color = Color.cyan;
            if(end != null) Gizmos.DrawSphere(end.data, 0.2f);

            if(path == null) return;

            Vector3? p = null;

            foreach(IPFNode<Vector3> node in path)
            {
                Gizmos.DrawSphere(node.data, 0.1f);
                if(p.HasValue) Gizmos.DrawLine(p.Value, node.data);
                p = node.data;
            }
        }

        [Button("Generate Pathfinding Octa-Tree")]
        private void GeneratePathFindingOctaTree()
        {
            treeGrid = networkData.ToPathFindingOctaTreeGrid();
        }

        [Button("Test Pathfinding")]
        private void TEST_Pathfinding(Vector3 from, Vector3 to)
        {
            pathfinding = new AStarPathFindingAlgorithm();
            start = treeGrid.GetClosestNode(from);
            end = treeGrid.GetClosestNode(to);
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