using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Voidless.Demos
{
    public class Demo_OctaTree : MonoBehaviour
    {
        [SerializeField] private Vector3 boundaries;
        [SerializeField] private Vector3[] points;
        [SerializeField] private BoundsOctaTree octaTree;

        /// <summary>Draws Gizmos on Editor mode when Demo_OctaTree's instance is selected.</summary>
        private void OnDrawGizmosSelected()
        {
            if(octaTree == null) return;
            
            Gizmos.color = Color.white;
            octaTree.DrawGizmos();
        }

        [Button("Generate Octa-Tree")]
        /// <summary>Generates BoundsOctaTree with the given amount of random points scattered across the boundaries.</summary>
        /// <param name="size">Size of set of points [20 by default].</param>
        private void GenerateBoundsOctaTree(int size = 20)
        {
            size = Mathf.Max(size, 20);

            float hx = Mathf.Abs(boundaries.x) * 0.5f;
            float hy = Mathf.Abs(boundaries.y) * 0.5f;
            float hz = Mathf.Abs(boundaries.z) * 0.5f;

            points = new Vector3[size];

            for(int i = 0; i < size; i++)
            {
                points[i] = VVector3.Random(-hx, hx, -hy, hy, -hz, hz);
            }

            octaTree = BoundsOctaTree.GenerateFromPoints(points);
        }
    }
}