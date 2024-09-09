using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
    [Serializable]
    public class QuadTree : /*ICollection<Rect>, */IEnumerable<QuadTree>, IEnumerable<Rect>
    {
        public const int MAX_POINTSPERNODE = 4;

        [SerializeField] private Rect _boundary;
        private List<Rect> _objects;
        private QuadTree[] _children;
        private bool _subdivided;
        
        /// <summary>Gets and Sets boundary property.</summary>
        public Rect boundary
        {
            get { return _boundary; }
            set { _boundary = value; }
        }

        /// <summary>Gets and Sets objects property.</summary>
        public List<Rect> objects
        {
            get { return _objects; }
            private set { _objects = value; }
        }

        /// <summary>Gets and Sets children property.</summary>
        public QuadTree[] children
        {
            get { return _children; }
            private set { _children = value; }
        }

        /// <summary>Gets and Sets subdivided property.</summary>
        public bool subdivided
        {
            get { return _subdivided; }
            private set { _subdivided = value; }
        }

        /// <returns>Collection's Count.</returns>
        public int Count { get { return children != null ? children.Length : 0; } }

        /// <returns>False [it is a write/read structure].</returns>
        public bool IsReadOnly { get { return false; } }

        /// <summary>ObjectQuadTree's constructor.</summary>
        /// <param name="_boundary">QuadTree's boundaries.</param>
        public QuadTree(Rect _boundary)
        {
            boundary = _boundary;
            objects = new List<Rect>();
            children = new QuadTree[MAX_POINTSPERNODE];
        }

        /// <summary>Tries to insert point into QuadTree.</summary>
        /// <param name="_point">Point to add.</param>
        /// <returns>True if point was successfully added (whether here or within children).</returns>
        public bool Insert(Vector2 _point)
        {
            if(!boundary.Contains(_point)) return false;
        
            Rect rect = new Rect(_point, Vector2.zero);

            /// Add the point if there is still room.
            if(objects.Count < MAX_POINTSPERNODE)
            {
                objects.Add(rect);
                return true;
            }

            /// Subdivide if necessary.
            if(!subdivided) Subdivide();

            foreach(QuadTree child in children)
            {
                if(child.Insert(_point)) return true;
            }

            return false;
        }

        /// <summary>Tries to insert object into QuadTree.</summary>
        /// <param name="_object">Rect to add.</param>
        /// <returns>True if object was successfully added (whether here or within children).</returns>
        public bool Insert(Rect _object)
        {
            if(!boundary.Contains(_object)) return false;

            /// Add the point if there is still room.
            if(objects.Count < MAX_POINTSPERNODE)
            {
                objects.Add(_object);
                return true;
            }

            /// Subdivide if necessary.
            if(!subdivided) Subdivide();

            foreach(QuadTree child in children)
            {
                if(child.Insert(_object)) return true;
            }

            return false;
        }

        /// <summary>Removes Point.</summary>
        /// <param name="_object">Point to remove [if it is contained within QuadTree].</param>
        /// <returns>True if it was successfully removed.</returns>
        public bool Remove(Rect _object)
        {
            // If the object doesn't belong in this boundary, return false
            if (!boundary.Contains(_object)) return false;

            // If the object is in this node, remove it
            if (objects.Remove(_object)) return true;

            // Try to remove the object from the children
            if (subdivided)
            {
                foreach (QuadTree child in children)
                {
                    if (child.Remove(_object)) return true;
                }
            }

            return false;
        }

        /// \TODO Improve this to be more efficient.
        /// <summary>Updates Object [if it did move from position].</summary>
        /// <param name="_object">Object to update.</param>
        public void UpdateObject(Rect _object)
        {
            Remove(_object);
            Insert(_object);
        }

        /// <summary>Subdivides QuadTree into 4 more sub-QuadTrees.</summary>
        private void Subdivide()
        {
            float hWidth = boundary.width * 0.5f;
            float hHeight = boundary.height * 0.5f;
            Vector2 c = boundary.center;

            children[0] = new QuadTree(new Rect(c.x, c.y, hWidth, hHeight)); // NE
            children[1] = new QuadTree(new Rect(c.x - hWidth, c.y, hWidth, hHeight)); // NW
            children[2] = new QuadTree(new Rect(c.x, c.y - hHeight, hWidth, hHeight)); // SE
            children[3] = new QuadTree(new Rect(c.x - hWidth, c.y - hHeight, hWidth, hHeight)); // SW

            subdivided = true;

            // Redistribute objects to children
            foreach (Rect obj in objects)
            {
                bool added = false;
                foreach (QuadTree child in children)
                {
                    if (child.Insert(obj))
                    {
                        added = true;
                        break;
                    }
                }
                // If the object can't fit in any child, leave it in the parent
                if (!added)
                {
                    break;
                }
            }

            // Clear the current objects if they are fully moved into children
            objects.Clear();
        }

        /// <summary>Gets Objets within given range.</summary>
        /// <param name="_range">Query's Range.</param>
        /// <param name="_found">List of found objects passed by reference.</param>
        /// <returns>List of found objects within range.</returns>
        public List<Rect> Query(Rect _range, ref List<Rect> _found, bool _resetList = true)
        {
            if(_found == null) _found = new List<Rect>();
            if(_resetList) _found.Clear();

            if(!VRect.Intersects(boundary, _range)) return _found;

            foreach(Rect obj in objects)
            {
                if(VRect.Contains(_range, obj)) _found.Add(obj);
            }

            if(subdivided) foreach(QuadTree child in children)
            {
                child.Query(_range, ref _found, false);
            }

            return _found;
        }

        /// <summary>Finds Neighbors at a given distance radius.</summary>
        /// <param name="_object">Reference object.</param>
        /// <param name="_distance">Distance radius.</param>
        /// <returns>Neighbors inside the distance radius of the referenced object.</returns>
        public List<Rect> FindNeighbors(Rect _object, float _distance, ref List<Rect> _neighbors)
        {
            float d = _distance * 2.0f;
            Rect searchArea = new Rect(_object.center, new Vector2(d, d));
            return Query(searchArea, ref _neighbors);
        }

        public void DrawGizmos()
        {
            VGizmos.DrawWireRect(boundary);
            
            if(objects != null) foreach(Rect obj in objects)
            {
                if(obj.size.sqrMagnitude > 0.1f) VGizmos.DrawWireRect(obj);
                else Gizmos.DrawSphere(obj.center, 0.05f);
            }

            if(children != null) foreach(QuadTree child in children)
            {
                if(child != null) child.DrawGizmos();
            }
        }

        public static QuadTree GenerateFromPoints(params Vector2[] points)
        {
            if(points == null || points.Length == 0) return null;

            Rect b = VRect.GetRectToFitSet(points);
            QuadTree tree = new QuadTree(b);

            foreach(Vector2 point in points)
            {
                tree.Insert(point);
            }

            return tree;
        }

#region InterfaceImplementations:
        /// <summary>Adds Item into QuadTree [internally calls for Insert without returning result].</summary>
        /// <param name="item">Item to add.</param>
        public void Add(Rect item)
        {
            Insert(item);
        }

        /// <summary>Clears Collection.</summary>
        public void Clear()
        {
            objects.Clear();
        }

        /// <returns>True if item is contained within QuadTree, or children if that's the case.</returns>
        public bool Contains(Rect item)
        {
            if(objects.Contains(item)) return true;

            foreach(QuadTree child in children)
            {
                if(child.Contains(item)) return true;
            }

            return false;
        }

        /// <summary>Copies element [NOT IMPLEMENTED].</summary>
        public void CopyTo(Rect[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        /// <returns>Iteration through collection of objects.</returns>
        public IEnumerator<Rect> GetEnumerator()
        {
            return objects.GetEnumerator();
        }

        /// <returns>Iteration through collection of objects.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return objects.GetEnumerator();
        }

        IEnumerator<QuadTree> IEnumerable<QuadTree>.GetEnumerator()
        {
            foreach (QuadTree child in children)
            {
                yield return child;
            }
        }
#endregion
    }
}