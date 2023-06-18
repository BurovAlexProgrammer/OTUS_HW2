using System.Collections.Generic;
using DI;
using Enemy;
using GameContext;
using Listener;
using Service;
using ShootEmUp;
using UnityEngine;

namespace Systems
{
    public sealed class EnemyController : MonoBehaviour, IUpdateListener
    {
        [SerializeField] private EnemyPositions enemyPositions;

        private EnemySpawner _enemySpawner;
        private BulletSpawner _bulletSpawner;
        private GameObject _character;

        private const float SpawnPeriod = 1.5f;

        [Inject]
        public void Construct(
            CharacterService characterService,
            BulletSpawner bulletSpawner,
            SceneContext sceneContext,
            EnemyPool enemyPool)
        {
            _character = characterService.Character;
            _bulletSpawner = bulletSpawner;
            _enemySpawner = new EnemySpawner(sceneContext.WorldTransform, enemyPool, SpawnPeriod);
            _enemySpawner.OnSpawned += OnSpawned;
            _enemySpawner.OnRequireSpawn += OnRequireSpawn;
        }

        private void OnRequireSpawn()
        {
            _enemySpawner.SpawnEnemy(enemyPositions.RandomSpawnPosition());
        }

        private void OnSpawned(GameObject enemy)
        {
            var attackPosition = enemyPositions.RandomAttackPosition();
            enemy.GetComponent<HitPointsComponent>().hpEmpty += OnDestroyed;
            enemy.GetComponent<EnemyAttackAgent>().OnFire += OnFire;
            enemy.GetComponent<EnemyMoveAgent>().SetDestination(attackPosition.position);
            enemy.GetComponent<EnemyAttackAgent>().SetTarget(_character);
        }

        private void OnDestroyed(GameObject enemy)
        {
            enemy.GetComponent<HitPointsComponent>().hpEmpty -= this.OnDestroyed;
            enemy.GetComponent<EnemyAttackAgent>().OnFire -= this.OnFire;
            _enemySpawner.UnspawnEnemy(enemy);
        }

        private void OnFire(GameObject enemy, Vector2 position, Vector2 direction)
        {
            _bulletSpawner.Spawn(new BulletSpawner.Args()
            {
                isPlayer = false,
                physicsLayer = (int)PhysicsLayer.ENEMY,
                color = Color.red,
                damage = 1,
                position = position,
                velocity = direction * 2.0f
            });
        }

        public void OnUpdate(float deltaTime)
        {
            _enemySpawner.Update(deltaTime);
        }
    }
}