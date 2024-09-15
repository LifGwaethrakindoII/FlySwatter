using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless.AI
{
    [Serializable]
    public class PathFindingGraph : IPathFindingGraph<Vector3>
    {
        [SerializeField] private Dictionary<Vector3, IPathFindingNode<Vector3>> _mapping;
        [SerializeField] private List<IPathFindingNode<Vector3>> _nodes;
        [SerializeField] private List<IPathFindingConnection<Vector3>> _connections;
        
        /// <summary>Gets and Sets mapping property.</summary>
        public Dictionary<Vector3, IPathFindingNode<Vector3>> mapping
        {
            get { return _mapping; }
            set { _mapping = value; }
        }

        /// <summary>Gets and Sets nodes property.</summary>
        public List<IPathFindingNode<Vector3>> nodes
        {
            get { return _nodes; }
            set { _nodes = value; }
        }

        /// <summary>Gets and Sets connections property.</summary>
        public List<IPathFindingConnection<Vector3>> connections
        {
            get { return _connections; }
            set { _connections = value; }
        }

        public virtual int Count => nodes != null ? nodes.Count : 0;

        public bool IsReadOnly => throw new NotImplementedException();

        public PathFindingGraph()
        {
            mapping = new Dictionary<Vector3, IPathFindingNode<Vector3>>();
            nodes = new List<IPathFindingNode<Vector3>>();
            connections = new List<IPathFindingConnection<Vector3>>();
        }

        /// <summary>Draws Gizmos.</summary>
        public virtual void DrawGizmos() { /*...*/ }

        public IPathFindingNode<Vector3> GetClosestNode(Vector3 _data)
        {
            if(Count == 0)
            {
                Debug.Log("Empty Shit...");
                return null;
            }

            IPathFindingNode<Vector3> closest = null;
            float minDistance = Mathf.Infinity;
            float threshold = 0.04f;

            foreach(IPathFindingNode<Vector3> node in this)
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

        public virtual IEnumerator<IPathFindingNode<Vector3>> GetEnumerator()
        {
            return nodes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(IPathFindingNode<Vector3> item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(IPathFindingNode<Vector3> item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(IPathFindingNode<Vector3>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(IPathFindingNode<Vector3> item)
        {
            throw new NotImplementedException();
        }
    }
}