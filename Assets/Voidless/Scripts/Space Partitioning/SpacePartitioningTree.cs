using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
    public abstract class SpacePartitioningTree<T, B> : ISpacePartitioningTree<T, B>
    {
        [SerializeField] protected B _boundary;
        [SerializeField] private bool _subdivided;
        [SerializeField] private HashSet<T> _objects;
        [SerializeField] private ISpacePartitioningTree<T, B>[] _children;
        [SerializeField] private Func<T, B> getObjectBoundary;
        [SerializeField] private Func<T, GizmosDrawParameters> getGizmosParemeters;

        /// <summary>Gets and Sets boundary property.</summary>
        public B boundary
        {
            get { return _boundary; }
            set { _boundary = value; }
        }

        /// <summary>Gets and Sets subdivided property.</summary>
        public bool subdivided
        {
            get { return _subdivided; }
            set { _subdivided = value; }
        }

        /// <summary>Gets and Sets objects property.</summary>
        public HashSet<T> objects
        {
            get { return _objects; }
            set { _objects = value; }
        }

        /// <summary>Gets and Sets children property.</summary>
        public ISpacePartitioningTree<T, B>[] children
        {
            get { return _children; }
            set { _children = value; }
        }

        /// <summary>Gets and Sets getObjectBoundary property.</summary>
        public Func<T, B> GetObjectBoundary
        {
            get { return getObjectBoundary; }
            set { getObjectBoundary = value; }
        }

        /// <summary>Gets & Sets getGizmosParemeters  property</summary>
        public Func<T, GizmosDrawParameters> GetGizmosParemeters 
        {
            get { return getGizmosParemeters ; }
            set { getGizmosParemeters  = value; }
        }

        /// <summary>Gets max children's capacity.</summary>
        public abstract int maxChildCapacity { get; }

        /// <summary>Gets max object's capacity.</summary>
        public abstract int maxObjectCapacity { get; }

        public SpacePartitioningTree(B _boundary, Func<T, B> getBoundary = null)
        {
            boundary = _boundary;
            GetObjectBoundary = getBoundary;
            objects = new HashSet<T>();
            children = new ISpacePartitioningTree<T, B>[maxChildCapacity];
        }

        /// <summary>Gets Position.</summary>
        /// <param name="p">Position [as Vector3].</param>
        public abstract Vector3 GetPosition();

        /// <summary>Gets Position.</summary>
        /// <param name="d">Dimensions [as Vector3].</param>
        public abstract Vector3 GetDimensions();

        /// <summary>Gets Position.</summary>
        /// <param name="p">Position [as Vector2].</param>
        public abstract Vector2 Get2DPosition();

        /// <summary>Gets Position.</summary>
        /// <param name="d">Dimensions [as Vector2].</param>
        public abstract Vector2 Get2DDimensions();

        /// <summary>Sets Position.</summary>
        /// <param name="p">Position [as Vector3].</param>
        public abstract void SetPosition(Vector3 p);

        /// <summary>Sets Position.</summary>
        /// <param name="d">Dimensions [as Vector3].</param>
        public abstract void SetDimensions(Vector3 d);

        /// <summary>Sets Position.</summary>
        /// <param name="p">Position [as Vector2].</param>
        public abstract void SetPosition(Vector2 p);

        /// <summary>Sets Position.</summary>
        /// <param name="d">Dimensions [as Vector2].</param>
        public abstract void SetDimensions(Vector2 d);

        /// <summary>Gets Position from Object.</summary>
        /// <param name="_object">Object's reference.</param>
        /// <param name="p">Object's position [as Vector3].</param>
        public abstract Vector3 GetObjectPosition(T _object);

        /// <summary>Gets Position from Object.</summary>
        /// <param name="_object">Object's reference.</param>
        /// <param name="d">Object's dimensions [as Vector3].</param>
        public abstract Vector3 GetObjectDimensions(T _object);

        /// <summary>Gets Position from Object.</summary>
        /// <param name="_object">Object's reference.</param>
        /// <param name="p">Object's position [as Vector2].</param>
        public abstract Vector2 GetObject2DPosition(T _object);

        /// <summary>Gets Position from Object.</summary>
        /// <param name="_object">Object's reference.</param>
        /// <param name="d">Object's dimensions [as Vector2].</param>
        public abstract Vector2 GetObject2DDimensions(T _object);

        /// <summary>Subdivides Tree.</summary>
        public abstract void Subdivide();

        /// <summary>Insertsobject into tree.</summary>
        /// <param name="_object">Object to insert.</param>
        /// <returns>True if object was successfully inserted.</returns>
        public virtual bool Insert(T _object)
        {
            if(!Contains(_object)) return false;

            if(objects.Count < maxObjectCapacity)
            {
                objects.Add(_object);
                return true;
            }

            if(!subdivided) Subdivide();

            foreach (ISpacePartitioningTree<T, B> child in children)
            {
                if(child.Insert(_object)) return true;
            }

            return false;
        }

        /// <summary>Removes object from tree or within children.</summary>
        /// <param name="_object">Object to remove.</param>
        /// <returns>True if object was successfully removed.</returns>
        public virtual bool Remove(T _object)
        {
            if(!Contains(_object)) return false;

            if(objects.Remove(_object)) return true;

            if(subdivided)
            {
                foreach (ISpacePartitioningTree<T, B> child in children)
                {
                    if(child.Remove(_object)) return true;
                }
            }

            return false;
        }

        /// <summary>Checks if tree, or children, contain provided object.</summary>
        /// <param name="_object">Object to evaluate.</param>
        /// <returns>True if object is contained within tree or children.</returns>
        public abstract bool Contains(T _object);

        /// <summary>Checks if tree, or children, contain provided object.</summary>
        /// <param name="_boundary">Boundar to evaluate.</param>
        /// <param name="_object">Object to evaluate.</param>
        /// <returns>True if object is contained within tree or children.</returns>
        public abstract bool Contains(B _boundary, T _object);

        /// <summary>Evaluates if 2 boundaries intersect.</summary>
        /// <param name="a">Boundary A.</param>
        /// <param name="b">Boundary B.</param>
        /// <returns>True if object intersects with tree's boundary</returns>
        public abstract bool Intersects(B a, B b);

        /// <summary>Evaluates if object intersects with this tree's boundary.</summary>
        /// <param name="_object">Object to evaluate.</param>
        /// <returns>True if object intersects with tree's boundary</returns>
        public abstract bool Intersects(T _object);

        /// <summary>Updates object.</summary>
        /// <param name="_object">Object to update</param>
        public virtual void UpdateObject(T _object)
        {
            Remove(_object);
            Insert(_object);
        }

        /// <summary>Retrieves all objects within a specific query range.</summary>
        /// <param name="range">Range to query.</param>
        /// <param name="_objects">Reference to List to hold found Bounds.</param>
        /// /// <param name="_resetList">Reset List? True by default.</param>
        /// <returns>List of found Bounds.</returns>
        public virtual List<T> Query(B _range, ref List<T> _objects, bool _resetList = true)
        {
            if(_objects == null) _objects = new List<T>();
            if(_resetList) _objects.Clear();

            if (!Intersects(boundary, _range)) return _objects;

            foreach (T obj in objects)
            {
                if (Contains(_range, obj)) _objects.Add(obj);
            }

            if (subdivided) foreach (ISpacePartitioningTree<T, B> child in children)
            {
                child.Query(_range, ref _objects, false);
            }

            return _objects;
        }

        /// <summary>Gets Neighbors from given object.</summary>
        /// <param name="_object">Refrence object.</param>
        /// <param name="_distance">Distance Radius.</param>
        /// /// <param name="_neighbors">Reference to List of found neighbors.</param>
        public abstract List<T> FindNeighbors(T _object, float _distance, ref List<T> _neighbors);

        /// <summary>Iterates through objects.</summary>
        /// <returns>Objects' iteration</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return objects != null ? objects.GetEnumerator() : null;
        }

        /// <summary>Iterates through objects.</summary>
        /// <returns>Objects' iteration</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>Draws Gizmos [use on either OnDrawGizmos or OnDrawGizmosSelected].</summary>
        public abstract void DrawGizmos();

        /// <summary>Callback invoked right after Subdivide.</summary>
        protected virtual void OnAfterSubdivided()
        {
            // Redistribute objects to children
            foreach (T obj in objects)
            {
                bool added = false;

                foreach (OctaTree<T> child in children)
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
    }
}