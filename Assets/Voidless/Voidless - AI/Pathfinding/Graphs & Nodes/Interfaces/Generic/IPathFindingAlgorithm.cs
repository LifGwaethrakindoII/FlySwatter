using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless.AI
{
    public interface IPathFindingAlgorithm<T>
    {
        public List<IPathFindingNode<T>> CalculatePath(IPathFindingNode<T> start, IPathFindingNode<T> end);
    }
}