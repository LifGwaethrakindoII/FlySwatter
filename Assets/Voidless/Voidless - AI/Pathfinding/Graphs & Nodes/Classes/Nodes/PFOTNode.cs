using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless.AI.PathFinding
{
    public class PFOTNode : PFNode
    {
        private Bounds _boundaries;

        /// <summary>Gets & Sets boundaries property.</summary>
        public Bounds boundaries
        {
            get { return _boundaries; }
            set { _boundaries = value; }
        }

        /// <summary>PathfindingNode's constructor.</summary>
        /// <param name="_data">Data</param>
        /// <param name="_traversable">Is it traversable? True by default.</param>
        /// <param name="_flags">Additional flags, none by default.</param>
        public PFOTNode(Vector3 _data, Bounds _boundaries, bool _traversable = true, int _flags = 0) : base(_data, _traversable, _flags)
        {
            boundaries = _boundaries;
        }
    }
}