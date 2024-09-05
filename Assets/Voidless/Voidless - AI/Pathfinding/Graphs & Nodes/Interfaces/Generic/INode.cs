using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless.AI
{
    public interface INode<T> : IInNode<T>, IOutNode<T>
    {
        INode<T> parent { get; set; }
    }
}