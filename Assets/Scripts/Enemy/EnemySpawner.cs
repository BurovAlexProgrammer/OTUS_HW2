using System;
using DI;
using ShootEmUp;
using Systems;
using UnityEngine;

namespace Enemy
{
    public sealed class EnemySpawner
    {
        public event Action OnRequireSpawn; 
        public event Action<GameObject> OnSpawned;
        
        private float _spawnTimer;
        private EnemyController _enemyController;
        private EnemyPool _enemyPool;
        private Transform _worldTransform;
        private float _spawnPeriod;
        
        public EnemySpawner(Transform worldTransform, EnemyPool enemyPool, float spawnPeriod)
        {
            _worldTransform = worldTransform;
            _enemyPool = enemyPool;
            _spawnPeriod = spawnPeriod;
        }

        public void SpawnEnemy(Transform spawnPoint)
        {
            var enemy = _enemyPool.SpawnEnemy(_worldTransform);
            
            if (enemy == null) return;
            
            enemy.transform.position = spawnPoint.position;
            OnSpawned?.Invoke(enemy);
        }

        public void UnspawnEnemy(GameObject enemy)
        {
            _enemyPool.UnspawnEnemy(enemy);
        }

        public void Update(float deltaTime)
        {
            _spawnTimer -= deltaTime;

            if (_spawnTimer <= 0f)
            {
                _spawnTimer = _spawnPeriod;
                OnRequireSpawn?.Invoke();
            }
        }
    }
}