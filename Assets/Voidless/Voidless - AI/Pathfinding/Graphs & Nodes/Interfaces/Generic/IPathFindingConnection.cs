using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless.AI
{
    public interface IPathFindingConnection<T>
    {
        IPathFindingNode<T> from { get; set; }
        IPathFindingNode<T> to { get; set; }
        float cost { get; set; }
        bool directed { get; set; }
    }
}