using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Voidless.AI
{
    public class PathFindingOctaTreeGrid : PathFindingGraph
    {
        private OctaTree<PathFindingOctaTreeNode> _nodeTree;

        /// <summary>Gets & Sets nodeTree property.</summary>
        public OctaTree<PathFindingOctaTreeNode> nodeTree
        {
            get { return _nodeTree; }
            set { _nodeTree = value; }
        }

        public override int Count => nodeTree != null && nodeTree.objects != null ? nodeTree.objects.Count : 0;

        public PathFindingOctaTreeGrid() : base()
        {
            nodeTree = new OctaTree<PathFindingOctaTreeNode>(default, n => n.boundaries);
        }

        /// <summary>Draws Gizmos.</summary>
        public override void DrawGizmos()
        {
            base.DrawGizmos();
            if (nodeTree != null)
            {
                nodeTree.DrawGizmos();
                Debug.Log("Debug..");
            }
        }
        public override IEnumerator<IPathFindingNode<Vector3>> GetEnumerator()
        {
            return nodeTree.GetEnumerator();
        }
    }
}