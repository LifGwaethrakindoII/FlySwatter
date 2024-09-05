using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless.AI
{
    public interface IPathFindingNode<T> : IInNode<T>, IOutNode<T>
    {
        IPathFindingNode<T> parent { get; set; }
        List<IPathFindingNode<T>> neighbors { get; set; }
        float gCost { get; set; }
        float hCost { get; set; }
        bool traversable { get; set; }
        int flags { get; set; }
    }
}