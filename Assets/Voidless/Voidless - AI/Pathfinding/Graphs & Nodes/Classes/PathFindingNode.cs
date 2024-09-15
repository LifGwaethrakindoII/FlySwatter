using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless.AI
{
    [Serializable]
    public class PathFindingNode : IPathFindingNode<Vector3>
    {
        [SerializeField] private IPathFindingNode<Vector3> _parent;
        [SerializeField] private List<IPathFindingNode<Vector3>> _neighbors;
        [SerializeField] private Vector3 _data;
        [SerializeField] private float _gCost;
        [SerializeField] private float _hCost;
        [SerializeField] private bool _traversable;
        [SerializeField] private int _flags;

        /// <summary>Gets and Sets data property.</summary>
        public Vector3 data { get { return _data; } }

        /// <summary>Sets data property.</summary>
        public void SetData(Vector3 _value) { _data = _value; }

        /// <summary>Gets and Sets parent property.</summary>
        public IPathFindingNode<Vector3> parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        /// <summary>Gets and Sets neighbors property.</summary>
        public List<IPathFindingNode<Vector3>> neighbors
        {
            get { return _neighbors; }
            set { _neighbors = value; }
        }

        /// <summary>Gets and Sets gCost property.</summary>
        public float gCost
        {
            get { return _gCost; }
            set { _gCost = value; }
        }

        /// <summary>Gets and Sets hCost property.</summary>
        public float hCost
        {
            get { return _hCost; }
            set { _hCost = value; }
        }

        /// <summary>Gets and Sets traversable property.</summary>
        public bool traversable
        {
            get { return _traversable; }
            set { _traversable = value; }
        }

        /// <summary>Gets and Sets flags property.</summary>
        public int flags
        {
            get { return _flags; }
            set { _flags = value; }
        }

        /// <summary>PathfindingNode's constructor.</summary>
        /// <param name="_data">Data</param>
        /// <param name="_traversable">Is it traversable? True by default.</param>
        /// <param name="_flags">Additional flags, none by default.</param>
        public PathFindingNode(Vector3 _data, bool _traversable = true, int _flags = 0)
        {
            SetData(_data);
            traversable = _traversable;
            flags = _flags;
        }
    }
}