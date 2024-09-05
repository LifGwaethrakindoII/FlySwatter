using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless.AI
{
    public interface IInNode<in T>
    {
        void SetData(T _value);
    }
}