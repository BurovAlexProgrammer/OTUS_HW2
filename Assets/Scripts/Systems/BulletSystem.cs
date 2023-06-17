using Listener;
using UnityEngine;

namespace ShootEmUp
{
    public sealed class BulletSystem : MonoBehaviour, IFixedUpdateListener, IGameInitListener
    {
        [SerializeField] private int initialCount = 50;
        [SerializeField] private Transform container;
        [SerializeField] private Bullet prefab;
        [SerializeField] private Transform worldTransform;
        [SerializeField] private LevelBounds levelBounds;

        private BulletPool _bulletPool = new BulletPool();
        private BulletSpawner _bulletSpawner = new BulletSpawner();
        private BulletTracker _bulletTracker = new BulletTracker();

        void IGameInitListener.OnInit()
        {
            _bulletPool.Init(prefab, container, initialCount, worldTransform);
            _bulletSpawner.Init(_bulletPool);
            _bulletPool.OnReturn += OnBulletReturnToPool;
            _bulletTracker.Init(levelBounds);
            _bulletTracker.OnRequireRemove += OnRequireRemoveBullet;
        }

        void IFixedUpdateListener.OnFixedUpdate(float fixedDeltaTime)
        {
            _bulletTracker.Track(_bulletPool);
        }

        public void Spawn(BulletSpawner.Args args)
        {
            var bullet = _bulletSpawner.Spawn(args);
            bullet.OnCollisionEntered += this.OnBulletCollision;
        }

        private void OnBulletCollision(Bullet bullet, Collision2D collision)
        {
            BulletUtils.DealDamage(bullet, collision.gameObject);
            this.RemoveBullet(bullet);
        }

        private void RemoveBullet(Bullet bullet)
        {
            _bulletPool.Return(bullet);
        }

        private void OnRequireRemoveBullet(Bullet bullet)
        {
            _bulletPool.Return(bullet);
        }

        private void OnBulletReturnToPool(Bullet bullet)
        {
            bullet.OnCollisionEntered -= this.OnBulletCollision;
        }
    }
}