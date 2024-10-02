using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Voidless.AI.PathFinding
{
    public class PFOctaTreeGrid : PFGraph
    {
        private OctaTree<PFOTNode> _nodeTree;

        /// <summary>Gets & Sets nodeTree property.</summary>
        public OctaTree<PFOTNode> nodeTree
        {
            get { return _nodeTree; }
            set { _nodeTree = value; }
        }

        public override int Count => nodeTree != null && nodeTree.objects != null ? nodeTree.objects.Count : 0;

        public PFOctaTreeGrid() : base()
        {
            nodeTree = new OctaTree<PFOTNode>(default, n => n.boundaries);
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
        public override IEnumerator<IPFNode<Vector3>> GetEnumerator()
        {
            return nodeTree.GetEnumerator();
        }
    }
}