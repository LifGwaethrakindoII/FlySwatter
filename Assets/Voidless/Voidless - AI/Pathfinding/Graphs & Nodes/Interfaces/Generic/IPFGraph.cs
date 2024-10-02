using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless.AI.PathFinding
{
    public interface IPFGraph<T> : IEnumerable<IPFNode<T>>, ICollection<IPFNode<T>>
    {
        IPFNode<T> GetClosestNode(T _data);
    }
}