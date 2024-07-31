using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless.FlySwatter
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private GameObjectPool<FloatingTextMeshProGUI> scoreFloatingTexts;
        private Enemy[] enemies;
        private int count;

        private void Awake()
        {
            enemies = FindObjectsOfType<Enemy>();            
        
            if(enemies != null)
            foreach(Enemy enemy in enemies)
            {
                enemy.onPoolObjectEvent += OnEnemyPoolObjectEvent;
            }

            count = enemies.Length;
            scoreFloatingTexts.Initialize();
        }

        private void OnEnemyPoolObjectEvent(IPoolObject _poolObject, PoolObjectEvent _event)
        {
            Enemy enemy = _poolObject as Enemy;

            if (enemy == null) return;

            switch (_event)
            {
                case PoolObjectEvent.Deactivated:
                    Vector3 direction = player.position - enemy.transform.position;
                    FloatingTextMeshProGUI textMesh = scoreFloatingTexts.Recycle(enemy.transform.position, Quaternion.LookRotation(direction));

                    if (textMesh != null) textMesh.textMesh.text = "+1!";

                    count++;
                    if(count >= enemies.Length)
                    {
                        foreach(Enemy e in enemies)
                        {
                            e.OnObjectRecycled();
                        }
                        count = 0;
                    }
                break;
            }
        }
    }
}