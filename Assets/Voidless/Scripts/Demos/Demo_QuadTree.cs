using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Voidless.Demos
{
    public class Demo_QuadTree : MonoBehaviour
    {
        [SerializeField] private Vector2 boundaries;
        [SerializeField] private Vector2[] points;
        [SerializeField] private QuadTree quadTree;

        /// <summary>Draws Gizmos on Editor mode when Demo_QuadTree's instance is selected.</summary>
        private void OnDrawGizmosSelected()
        {
            if(quadTree == null) return;
            
            Gizmos.color = Color.white;
            quadTree.DrawGizmos();
        }

        [Button("Generate Quad-Tree")]
        /// <summary>Generates QuadTree with the given amount of random points scattered across the boundaries.</summary>
        /// <param name="size">Size of set of points [20 by default].</param>
        private void GenerateQuadTree(int size = 20)
        {
            size = Mathf.Max(size, 20);

            float hx = Mathf.Abs(boundaries.x) * 0.5f;
            float hy = Mathf.Abs(boundaries.y) * 0.5f;

            points = new Vector2[size];

            for(int i = 0; i < size; i++)
            {
                points[i] = VVector2.Random(-hx, hx, -hy, hy);
            }

            quadTree = QuadTree.GenerateFromPoints(points);
        }
    }
}