using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
    - Subdivide
    - FindNeighbors
    - Insert
    - Remove
    - UpdateObject
 */
namespace Voidless
{
    public interface IOctaTree<T, B> : IEnumerable<T>
    {
        IOctaTree<T, B>[] children { get; set; }

        HashSet<T> objects { get; set; }

        B bounds { get; set; }
        bool subdivided { get; set; }

        void Subdivide();

        bool Insert(T _object);
        bool Remove(T _object);

        bool Contains(T _object);

        bool ContainsAll(IEnumerable<T> _objects);

        bool RemoveAll(IEnumerable<T> _object);

        bool RemoveAny(IEnumerable<T> _objects, Predicate<T> condition);

        List<T> Query(B _range, ref List<T> _objects);

        List<T> Query(B _range, ref List<T> _objects, Predicate<T> condition);

        List<T> FindNeighbors(T _object, float _distance);
    }
}