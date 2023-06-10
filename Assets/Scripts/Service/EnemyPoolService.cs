using System.Collections.Generic;
using Service;
using UnityEngine;

namespace ShootEmUp
{
    public sealed class EnemyPoolService : ServiceBase
    {
        [Header("Pool")]
        [SerializeField]
        private Transform container;

        [SerializeField]
        private GameObject prefab;

        private readonly Queue<GameObject> enemyPool = new();
        
        private void Awake()
        {
            for (var i = 0; i < 7; i++)
            {
                var enemy = Instantiate(this.prefab, this.container);
                this.enemyPool.Enqueue(enemy);
            }
        }

        public GameObject SpawnEnemy(Transform worldTransform)
        {
            if (!this.enemyPool.TryDequeue(out var enemy))
            {
                return null;
            }

            enemy.transform.SetParent(worldTransform);

            return enemy;
        }

        public void UnspawnEnemy(GameObject enemy)
        {
            enemy.transform.SetParent(this.container);
            this.enemyPool.Enqueue(enemy);
        }
    }
}