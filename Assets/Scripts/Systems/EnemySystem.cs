using System.Collections.Generic;
using DI;
using Listener;
using Service;
using ShootEmUp;
using UnityEngine;

namespace Systems
{
    public sealed class EnemySystem : MonoBehaviour, IGameStartListener, IGameInitListener, IUpdateListener
    {
        [SerializeField] private Transform _worldTransform;
        [SerializeField] private EnemyPositions enemyPositions;

        private EnemyPoolService _enemyPoolService;
        private BulletSpawnService _bulletSpawnService;
        private readonly HashSet<GameObject> _activeEnemies = new();
        private float _spawnTimer;
        private GameObject _character;

        private const float SpawnPeriod = 1.5f;

        [Inject]
        public void Construct(CharacterService characterService, EnemyPoolService enemyPoolService, BulletSpawnService bulletSpawnService)
        {
            _character = characterService.Character;
            _enemyPoolService = enemyPoolService;
            _bulletSpawnService = bulletSpawnService;
        }
        
        private void OnDestroyed(GameObject enemy)
        {
            if (_activeEnemies.Remove(enemy))
            {
                enemy.GetComponent<HitPointsComponent>().hpEmpty -= this.OnDestroyed;
                enemy.GetComponent<EnemyAttackAgent>().OnFire -= this.OnFire;

                _enemyPoolService.UnspawnEnemy(enemy);
            }
        }

        private void OnFire(GameObject enemy, Vector2 position, Vector2 direction)
        {
            _bulletSpawnService.Spawn(new BulletSpawnService.Args()
            {
                isPlayer = false,
                physicsLayer = (int) PhysicsLayer.ENEMY,
                color = Color.red,
                damage = 1,
                position = position,
                velocity = direction * 2.0f
            });
        }

        public void OnStart()
        {
            
        }

        public void OnInit()
        {
            
        }

        public void OnUpdate(float deltaTime)
        {
            _spawnTimer -= deltaTime;
            
            if (_spawnTimer <= 0f)
            {
                _spawnTimer = SpawnPeriod;
                SpawnEnemy();
            }
        }

        private void SpawnEnemy()
        {
            var enemy = this._enemyPoolService.SpawnEnemy(_worldTransform);
            if (enemy != null)
            {
                if (this._activeEnemies.Add(enemy))
                {
                    var attackPosition = this.enemyPositions.RandomAttackPosition();
                    enemy.transform.position = enemyPositions.RandomSpawnPosition().position;
                    enemy.GetComponent<HitPointsComponent>().hpEmpty += this.OnDestroyed;
                    enemy.GetComponent<EnemyAttackAgent>().OnFire += this.OnFire;
                    enemy.GetComponent<EnemyMoveAgent>().SetDestination(attackPosition.position);
                    enemy.GetComponent<EnemyAttackAgent>().SetTarget(_character);
                    
                    var spawnPosition = this.enemyPositions.RandomSpawnPosition();
                }    
            }
        }
    }
}