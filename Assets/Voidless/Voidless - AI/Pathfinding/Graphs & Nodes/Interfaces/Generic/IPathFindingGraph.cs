using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless.AI
{
    public interface IPathFindingGraph<T> : IEnumerable<IPathFindingNode<T>>, ICollection<IPathFindingNode<T>>
    {
        /*Dictionary<T, IPathFindingNode<T>> mapping { get; set; }
        List<IPathFindingNode<T>> nodes { get; set; }
        List<IPathFindingConnection<T>> connections { get; set; }*/

        IPathFindingNode<T> GetClosestNode(T _data);
    }
}