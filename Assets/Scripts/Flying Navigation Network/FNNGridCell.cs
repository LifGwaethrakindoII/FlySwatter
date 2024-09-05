using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless.FlySwatter
{
    [Serializable]
    public struct FNNGridCell
    {
        [SerializeField] private Vector3 _position;
        [SerializeField] private bool _flyable;
    
        /// <summary>Gets and Sets position property.</summary>
        public Vector3 position
        {
            get { return _position; }
            set { _position = value; }
        }

        /// <summary>Gets and Sets flyable property.</summary>
        public bool flyable
        {
            get { return _flyable; }
            set { _flyable = value; }
        }

        public FNNGridCell(Vector3 _position, bool _flyable) : this()
        {
            position = _position;
            flyable = _flyable;
        }
    }
}