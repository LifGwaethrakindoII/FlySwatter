using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
    [Serializable]
    public class OctaTree : IEnumerable<OctaTree>, IEnumerable<Bounds>
    {
        public const int MAX_OBJECTS_PER_NODE = 8;

        [SerializeField] private Bounds _boundary;
        private List<Bounds> _objects;
        private OctaTree[] _children;
        private bool _subdivided;

        /// <summary>Gets and Sets boundary property.</summary>
        public Bounds boundary
        {
            get { return _boundary; }
            set { _boundary = value; }
        }

        /// <summary>Gets and Sets objects property.</summary>
        public List<Bounds> objects
        {
            get { return _objects; }
            private set { _objects = value; }
        }

        /// <summary>Gets and Sets children property.</summary>
        public OctaTree[] children
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

        /// <summary>OctaTree constructor.</summary>
        /// <param name="_boundary">Boundary for the OctaTree node.</param>
        public OctaTree(Bounds _boundary)
        {
            boundary = _boundary;
            objects = new List<Bounds>();
            children = new OctaTree[8];
        }

        /// <summary>Inserts a point into the OctaTree by converting it into a zero-size Bounds.</summary>
        /// <param name="point">Point to insert.</param>
        /// <returns>True if successfully inserted, false otherwise.</returns>
        public bool Insert(Vector3 point)
        {
            // Convert point into zero-size Bounds
            return Insert(new Bounds(point, Vector3.zero));
        }

        /// <summary>Inserts a Bounds into the OctaTree.</summary>
        /// <param name="bounds">Bounds to insert.</param>
        /// <returns>True if successfully inserted, false otherwise.</returns>
        public bool Insert(Bounds bounds)
        {
            // If the bounds are not within this node, return false
            if (!boundary.Intersects(bounds)) return false;

            // If there's room in this node and it hasn't reached capacity
            if (objects.Count < MAX_OBJECTS_PER_NODE)
            {
                objects.Add(bounds);
                return true;
            }

            // Otherwise, subdivide if not already subdivided
            if (!subdivided) Subdivide();

            // Try to insert into children
            foreach (OctaTree child in children)
            {
                if (child.Insert(bounds)) return true;
            }

            return false;
        }

        /// <summary>Subdivides the OctaTree into 8 smaller regions.</summary>
        private void Subdivide()
        {
            Vector3 s = boundary.size * 0.5f;
            Vector3 d = s * 0.5f;
            Vector3 c = boundary.center;

            // Create 8 sub-regions (octants)
            children[0] = new OctaTree(new Bounds(c + new Vector3(d.x, d.y, d.z), s));  // NEU
            children[1] = new OctaTree(new Bounds(c + new Vector3(-d.x, d.y, d.z), s)); // NWU
            children[2] = new OctaTree(new Bounds(c + new Vector3(d.x, -d.y, d.z), s)); // SEU
            children[3] = new OctaTree(new Bounds(c + new Vector3(-d.x, -d.y, d.z), s)); // SWU
            children[4] = new OctaTree(new Bounds(c + new Vector3(d.x, d.y, -d.z), s));  // NED
            children[5] = new OctaTree(new Bounds(c + new Vector3(-d.x, d.y, -d.z), s)); // NWD
            children[6] = new OctaTree(new Bounds(c + new Vector3(d.x, -d.y, -d.z), s)); // SED
            children[7] = new OctaTree(new Bounds(c + new Vector3(-d.x, -d.y, -d.z), s)); // SWD

            subdivided = true;
        }

        /// <summary>Retrieves all Bounds objects within a specific query range.</summary>
        /// <param name="range">Range to query.</param>
        /// <param name="foundObjects">List to hold found Bounds.</param>
        /// <returns>List of found Bounds.</returns>
        public List<Bounds> Query(Bounds range, ref List<Bounds> foundObjects)
        {
            if (foundObjects == null) foundObjects = new List<Bounds>();

            // If the range does not intersect this node, return empty
            if (!boundary.Intersects(range)) return foundObjects;

            // Check objects in this node
            foreach (Bounds obj in objects)
            {
                if (range.Intersects(obj)) foundObjects.Add(obj);
            }

            // Query the children if the node is subdivided
            if (subdivided)
            {
                foreach (OctaTree child in children)
                {
                    child.Query(range, ref foundObjects);
                }
            }

            return foundObjects;
        }

        /// <summary>Generates an Octa-Tree from a set of points.</summary>
        /// <param name="points">Set of Points.</param>
        public static OctaTree GenerateFromPoints(params Vector3[] points)
        {
            if (points == null || points.Length == 0) return null;

            Bounds b = VBounds.GetBoundsToFitSet(points);
            OctaTree tree = new OctaTree(b);

            foreach (Vector3 point in points)
            {
                tree.Insert(point);
            }

            return tree;
        }

        /// <summary>Implementation of IEnumerable for OctaTree, enabling iteration through child nodes.</summary>
        /// <returns>Enumerator for OctaTree.</returns>
        public IEnumerator<OctaTree> GetEnumerator()
        {
            yield return this;

            if (subdivided)
            {
                foreach (OctaTree child in children)
                {
                    foreach (OctaTree subChild in child)
                    {
                        yield return subChild;
                    }
                }
            }
        }

        /// <summary>Implementation of IEnumerable for Bounds, enabling iteration through objects.</summary>
        /// <returns>Enumerator for Bounds.</returns>
        IEnumerator<Bounds> IEnumerable<Bounds>.GetEnumerator()
        {
            foreach (Bounds obj in objects)
            {
                yield return obj;
            }

            if (subdivided)
            {
                foreach (OctaTree child in children)
                {
                    foreach (Bounds obj in child.objects)
                    {
                        yield return obj;
                    }
                }
            }
        }

        /// <summary>Non-generic IEnumerator implementation.</summary>
        /// <returns>Non-generic IEnumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>Draws Gizmos for visualization in the editor.</summary>
        public void DrawGizmos()
        {
            Gizmos.DrawWireCube(boundary.center, boundary.size);

            // Draw objects
            foreach (Bounds obj in objects)
            {
                if (obj.size.sqrMagnitude > 0.01f) Gizmos.DrawWireCube(obj.center, obj.size);
                else Gizmos.DrawSphere(obj.center, 0.2f);
            }

            // Draw children
            if (subdivided)
            {
                foreach (OctaTree child in children)
                {
                    child.DrawGizmos();
                }
            }
        }
    }
}
