using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

/*===========================================================================
**
** Class:  PFGraph
**
** Purpose: Graph for PathFinding.
**
**
** Author: Lîf Gwaethrakindo
**
===========================================================================*/
/// \TODO Create a PFGraph2D that has a QuadTree instead of OctaTree.
namespace Voidless.AI.PathFinding
{
    [Serializable]
    public class PFGraph : IPFGraph<Vector3>
    {
        [SerializeField] private Dictionary<Vector3, IPFNode<Vector3>> _mapping;
        [SerializeField] private List<IPFNode<Vector3>> _nodes;
        [SerializeField] private List<IPFConnection<Vector3>> _connections;
        [SerializeField] private OctaTree<PFOTNode> _octaTree;
        
        /// <summary>Gets and Sets mapping property.</summary>
        public Dictionary<Vector3, IPFNode<Vector3>> mapping
        {
            get { return _mapping; }
            set { _mapping = value; }
        }

        /// <summary>Gets and Sets nodes property.</summary>
        public List<IPFNode<Vector3>> nodes
        {
            get { return _nodes; }
            set { _nodes = value; }
        }

        /// <summary>Gets and Sets connections property.</summary>
        public List<IPFConnection<Vector3>> connections
        {
            get { return _connections; }
            set { _connections = value; }
        }

        /// <summary>Gets and Sets octaTree property.</summary>
        public OctaTree<PFOTNode> octaTree
        {
            get { return _octaTree; }
            set { _octaTree = value; }
        }

        public virtual int Count => nodes != null ? nodes.Count : 0;

        public bool IsReadOnly => throw new NotImplementedException();

        public PFGraph()
        {
            mapping = new Dictionary<Vector3, IPFNode<Vector3>>();
            nodes = new List<IPFNode<Vector3>>();
            connections = new List<IPFConnection<Vector3>>();
        }

        /// <summary>Draws Gizmos.</summary>
        public virtual void DrawGizmos() { /*...*/ }

        public IPFNode<Vector3> GetClosestNode(Vector3 _data)
        {
            if(Count == 0)
            {
                Debug.Log("Empty Shit...");
                return null;
            }

            IPFNode<Vector3> closest = null;
            float minDistance = Mathf.Infinity;
            float threshold = 0.04f;

            foreach(IPFNode<Vector3> node in this)
            {
                float sqrDistance = VVector3.SqrDistance(_data, node.data);
                if(sqrDistance < minDistance)
                {
                    Debug.Log("Shit's closer");
                    minDistance = sqrDistance;
                    closest = node;

                    if(minDistance <= threshold) return closest;
                }
            }

            return closest;
        }

        public virtual IEnumerator<IPFNode<Vector3>> GetEnumerator()
        {
            return nodes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(IPFNode<Vector3> item)
        {
            PFOTNode node = item as PFOTNode;
            octaTree.Insert(node);
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(IPFNode<Vector3> item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(IPFNode<Vector3>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(IPFNode<Vector3> item)
        {
            throw new NotImplementedException();
        }
    }
}