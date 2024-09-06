using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
    [Serializable]
    public class QuadTree
    {
        public const int MAX_POINTSPERNODE = 4;

        [SerializeField/*, HideInInspector*/] private Rect _boundary;
        [SerializeField/*, HideInInspector*/] private List<Rect> _objects;
        [SerializeField/*, HideInInspector*/] private QuadTree[] _children;
        [SerializeField/*, HideInInspector*/] private bool _subdivided;
        
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

        public QuadTree(Rect _boundary)
        {
            boundary = _boundary;
            objects = new List<Rect>();
            children = new QuadTree[MAX_POINTSPERNODE];
        }

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
        }

        public List<Rect> Query(Rect _range)
        {
            List<Rect> found = new List<Rect>();

            if(!VRect.Intersects(boundary, _range)) return found;

            foreach(Rect obj in objects)
            {
                if(VRect.Contains(_range, obj)) found.Add(obj);
            }

            if(subdivided) foreach(QuadTree child in children)
            {
                found.AddRange(child.Query(_range));
            }

            return found;
        }

        public void DrawGizmos()
        {
            VGizmos.DrawWireRect(boundary);
            
            if(objects != null) foreach(Rect obj in objects)
            {
                if(obj.size.sqrMagnitude > 0.1f)
                VGizmos.DrawWireRect(obj);
                else
                Gizmos.DrawSphere(obj.center, 0.05f);
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
    }
}