using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*===========================================================================
**
** Class:  Serializable3DArray
**
** Purpose: Serializable class that wraps a T[,,] array.
**
**
** Author: Lîf Gwaethrakindo
**
===========================================================================*/
namespace Voidless
{
    [Serializable]
    public class Serializable3DArray<T> : ISerializationCallbackReceiver, IEnumerable<T>, ICollection<T>, IList<T>, ICloneable, IComparable<Serializable3DArray<T>>, IEquatable<Serializable3DArray<T>>, IStructuralComparable, IStructuralEquatable
    {
        [SerializeField/*, HideInInspector*/] private T[] flattenedArray;
        private T[,,] array;
        private int width, height, depth;

        public int Length => flattenedArray.Length;

        public int Count => flattenedArray.Length;

        public bool IsReadOnly => false;

        public T this[int x, int y, int z]
        {
            get { return array[x, y, z]; }
            set { array[x, y, z] = value; }
        }

        public T this[Vector3Int i]
        {
            get { return this[i.x, i.y, i.z]; }
            set { this[i.x, i.y, i.z] = value; }
        }

        public T this[int index]
        {
            get { return flattenedArray[index]; }
            set { flattenedArray[index] = value; }
        }

        public Serializable3DArray(int x, int y, int z)
        {
            array = new T[x, y, z];
            flattenedArray = new T[x * y * z];
            width = x;
            height = y;
            depth = z;
        }

        /// <summary>Returns length of the row by provided index.</summary>
        /// <param name="i">Row's index [must be from 0 to 2].</param>
        /// <returns>Row by given index.</returns>
        public int GetLength(int i)
        {
            return array != null ? array.GetLength(i) : 0;
        }

        public void OnBeforeSerialize()
        {
            if(array == null) return;

            if(flattenedArray == null) flattenedArray = new T[width * height * depth];

            int index = 0;
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    for (int k = 0; k < array.GetLength(2); k++)
                    {
                        flattenedArray[index++] = array[i, j, k];
                    }
                }
            }
        }

        public void OnAfterDeserialize()
        {
            if(array == null) array = new T[width, height, depth];

            int index = 0;
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    for (int k = 0; k < array.GetLength(2); k++)
                    {
                        array[i, j, k] = flattenedArray[index++];
                    }
                }
            }
        }

        // IEnumerable<T> implementation
        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in flattenedArray)
            {
                yield return item;
            }
        }

        // IEnumerable implementation
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // ICollection<T> implementation
        public void Add(T item)
        {
            throw new NotSupportedException("Cannot add an item to a fixed-size 3D array.");
        }

        public void Clear()
        {
            Array.Clear(flattenedArray, 0, flattenedArray.Length);
            Array.Clear(array, 0, array.Length);
        }

        public bool Contains(T item)
        {
            return Array.IndexOf(flattenedArray, item) >= 0;
        }

        public void CopyTo(T[] targetArray, int arrayIndex)
        {
            Array.Copy(flattenedArray, 0, targetArray, arrayIndex, flattenedArray.Length);
        }

        public bool Remove(T item)
        {
            throw new NotSupportedException("Cannot remove an item from a fixed-size 3D array.");
        }

        // IList<T> implementation
        /// <summary>Returns index of provided item.</summary>
        /// <param name="item">Item's reference/value</param>
        /// <returns>Index of provided reference/value.</returns>
        public int IndexOf(T item)
        {
            return Array.IndexOf(flattenedArray, item);
        }

        public void Insert(int index, T item)
        {
            throw new NotSupportedException("Cannot insert an item into a fixed-size 3D array.");
        }

        public void RemoveAt(int index)
        {
            throw new NotSupportedException("Cannot remove an item from a fixed-size 3D array.");
        }

        // ICloneable implementation
        public object Clone()
        {
            Serializable3DArray<T> clone = new Serializable3DArray<T>(array.GetLength(0), array.GetLength(1), array.GetLength(2));
            Array.Copy(flattenedArray, clone.flattenedArray, flattenedArray.Length);
            clone.OnAfterDeserialize(); // To populate the 3D array from the flattened array
            return clone;
        }

        // IComparable<Serializable3DArray<T>> implementation
        public int CompareTo(Serializable3DArray<T> other)
        {
            if (other == null) return 1;
            return flattenedArray.Length.CompareTo(other.flattenedArray.Length);
        }

        // IEquatable<Serializable3DArray<T>> implementation
        public bool Equals(Serializable3DArray<T> other)
        {
            if (other == null || flattenedArray.Length != other.flattenedArray.Length) return false;
            for (int i = 0; i < flattenedArray.Length; i++)
            {
                if (!EqualityComparer<T>.Default.Equals(flattenedArray[i], other.flattenedArray[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Serializable3DArray<T>);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            foreach (var item in flattenedArray)
            {
                hash = hash * 31 + EqualityComparer<T>.Default.GetHashCode(item);
            }
            return hash;
        }

        // IStructuralComparable implementation
        public int CompareTo(object other, IComparer comparer)
        {
            if (other is Serializable3DArray<T> otherArray)
            {
                if (flattenedArray.Length != otherArray.flattenedArray.Length)
                    return flattenedArray.Length.CompareTo(otherArray.flattenedArray.Length);

                for (int i = 0; i < flattenedArray.Length; i++)
                {
                    int comparison = comparer.Compare(flattenedArray[i], otherArray.flattenedArray[i]);
                    if (comparison != 0) return comparison;
                }

                return 0;
            }
            throw new ArgumentException("Object is not a Serializable3DArray<T>");
        }

        // IStructuralEquatable implementation
        public bool Equals(object other, IEqualityComparer comparer)
        {
            if (other is Serializable3DArray<T> otherArray && flattenedArray.Length == otherArray.flattenedArray.Length)
            {
                for (int i = 0; i < flattenedArray.Length; i++)
                {
                    if (!comparer.Equals(flattenedArray[i], otherArray.flattenedArray[i]))
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        public int GetHashCode(IEqualityComparer comparer)
        {
            int hash = 17;
            foreach (var item in flattenedArray)
            {
                hash = hash * 31 + comparer.GetHashCode(item);
            }
            return hash;
        }
    }
}
