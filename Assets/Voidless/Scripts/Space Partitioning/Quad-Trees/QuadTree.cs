using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
    [Serializable]
    public class QuadTree<T> : SpacePartitioningTree<T, Rect>
    {
        public const int MAX_POINTSPERNODE = 4;

        /// <summary>Gets max children's capacity.</summary>
        public override int maxChildCapacity { get { return MAX_POINTSPERNODE; } }

        /// <summary>Gets max object's capacity.</summary>
        public override int maxObjectCapacity { get { return MAX_POINTSPERNODE; } }

        /// <summary>ObjectRectQuadTree's constructor.</summary>
        /// <param name="_boundary">RectQuadTree's boundaries.</param>
        public QuadTree(Rect _boundary, Func<T, Rect> getRect = null) : base(_boundary, getRect) { /*...*/ }

        /// <summary>Gets Position.</summary>
        /// <param name="p">Position [as Vector3].</param>
        public override Vector3 GetPosition() { return boundary.center; }

        /// <summary>Gets Position.</summary>
        /// <param name="d">Dimensions [as Vector3].</param>
        public override Vector3 GetDimensions() { return boundary.size; }

        /// <summary>Gets Position.</summary>
        /// <param name="p">Position [as Vector2].</param>
        public override Vector2 Get2DPosition() { return boundary.center; }

        /// <summary>Gets Position.</summary>
        /// <param name="d">Dimensions [as Vector2].</param>
        public override Vector2 Get2DDimensions() { return boundary.size; }

        /// <summary>Sets Position.</summary>
        /// <param name="p">Position [as Vector3].</param>
        public override void SetPosition(Vector3 p) { _boundary.position = p; }

        /// <summary>Sets Position.</summary>
        /// <param name="d">Dimensions [as Vector3].</param>
        public override void SetDimensions(Vector3 d) { _boundary.size = d; }

        /// <summary>Sets Position.</summary>
        /// <param name="p">Position [as Vector2].</param>
        public override void SetPosition(Vector2 p) { _boundary.position = p; }

        /// <summary>Sets Position.</summary>
        /// <param name="d">Dimensions [as Vector2].</param>
        public override void SetDimensions(Vector2 d) { _boundary.size = d; }

        /// <summary>Gets Position from Object.</summary>
        /// <param name="_object">Object's reference.</param>
        /// <param name="p">Object's position [as Vector3].</param>
        public override Vector3 GetObjectPosition(T _object) { return GetObjectBoundary != null ? GetObjectBoundary(_object).position : Vector3.zero; }

        /// <summary>Gets Position from Object.</summary>
        /// <param name="_object">Object's reference.</param>
        /// <param name="d">Object's dimensions [as Vector3].</param>
        public override Vector3 GetObjectDimensions(T _object) { return GetObjectBoundary != null ? GetObjectBoundary(_object).size : Vector3.zero; }

        /// <summary>Gets Position from Object.</summary>
        /// <param name="_object">Object's reference.</param>
        /// <param name="p">Object's position [as Vector2].</param>
        public override Vector2 GetObject2DPosition(T _object) { return GetObjectBoundary != null ? GetObjectBoundary(_object).position : Vector3.zero; }

        /// <summary>Gets Position from Object.</summary>
        /// <param name="_object">Object's reference.</param>
        /// <param name="d">Object's dimensions [as Vector2].</param>
        public override Vector2 GetObject2DDimensions(T _object) { return GetObjectBoundary != null ? GetObjectBoundary(_object).size : Vector3.zero; }

        /// <summary>Subdivides QuadTree into 4 more sub-QuadTrees.</summary>
        public override void Subdivide()
        {
            float hWidth = boundary.width * 0.5f;
            float hHeight = boundary.height * 0.5f;
            Vector2 c = boundary.center;

            children[0] = new QuadTree<T>(new Rect(c.x, c.y, hWidth, hHeight), GetObjectBoundary); // NE
            children[1] = new QuadTree<T>(new Rect(c.x - hWidth, c.y, hWidth, hHeight), GetObjectBoundary); // NW
            children[2] = new QuadTree<T>(new Rect(c.x, c.y - hHeight, hWidth, hHeight), GetObjectBoundary); // SE
            children[3] = new QuadTree<T>(new Rect(c.x - hWidth, c.y - hHeight, hWidth, hHeight), GetObjectBoundary); // SW

            subdivided = true;
            OnAfterSubdivided();
        }

        /// <summary>Checks if tree, or children, contain provided object.</summary>
        /// <param name="_object">Object to evaluate.</param>
        /// <returns>True if object is contained within tree or children.</returns>
        public override bool Contains(T _object)
        {
            return GetObjectBoundary != null ? boundary.Contains(GetObjectBoundary(_object)) : false;
        }

        /// <summary>Checks if tree, or children, contain provided object.</summary>
        /// <param name="_boundary">Boundar to evaluate.</param>
        /// <param name="_object">Object to evaluate.</param>
        /// <returns>True if object is contained within tree or children.</returns>
        public override bool Contains(Rect _boundary, T _object)
        {
            return GetObjectBoundary != null ? _boundary.Contains(GetObjectBoundary(_object)) : false;
        }

        /// <summary>Evaluates if object intersects with this tree's boundary.</summary>
        /// <param name="_object">Object to evaluate.</param>
        /// <returns>True if object intersects with tree's boundary</returns>
        public override bool Intersects(T _object)
        {
            return GetObjectBoundary != null ? VRect.Intersects(boundary, GetObjectBoundary(_object)) : false;
        }

        /// <summary>Evaluates if 2 boundaries intersect.</summary>
        /// <param name="a">Boundary A.</param>
        /// <param name="b">Boundary B.</param>
        /// <returns>True if object intersects with tree's boundary</returns>
        public override bool Intersects(Rect a, Rect b)
        {
            return VRect.Intersects(a, b);
        }

        /// <summary>Gets Neighbors from given object.</summary>
        /// <param name="_object">Refrence object.</param>
        /// <param name="_distance">Distance Radius.</param>
        /// /// <param name="_neighbors">Reference to List of found neighbors.</param>
        public override List<T> FindNeighbors(T _object, float _distance, ref List<T> _neighbors)
        {
            float d = _distance * 2.0f;
            Rect searchArea = new Rect(GetObjectBoundary(_object).center, new Vector2(d, d));
            return Query(searchArea, ref _neighbors);
        }

        /// <summary>Draws Gizmos [use on either OnDrawGizmos or OnDrawGizmosSelected].</summary>
        public override void DrawGizmos()
        {
            VGizmos.DrawWireRect(boundary);
            
            if(objects != null) foreach(T obj in objects)
            {
                Rect boundaries = GetObjectBoundary(obj);

                if(boundaries.size.sqrMagnitude > 0.1f) VGizmos.DrawWireRect(boundaries);
                else Gizmos.DrawSphere(boundaries.center, 0.05f);
            }

            if(children != null) foreach(QuadTree<T> child in children)
            {
                if(child != null) child.DrawGizmos();
            }
        }
    }
}