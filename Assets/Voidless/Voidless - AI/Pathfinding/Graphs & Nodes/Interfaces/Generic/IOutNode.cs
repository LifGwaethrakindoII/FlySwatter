using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless.AI
{
    public interface IOutNode<out T>
    {
        public T data { get; }
    }
}