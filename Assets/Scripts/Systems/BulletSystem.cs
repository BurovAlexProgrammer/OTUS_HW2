using DI;
using Listener;
using Service;
using UnityEngine;

namespace ShootEmUp
{
    public sealed class BulletSystem : MonoBehaviour, IFixedUpdateListener, IGameInitListener
    {
        private BulletSpawner _bulletSpawner;
        private BulletTracker _bulletTracker = new BulletTracker();

        [Inject]
        public void Construct(BulletSpawner bulletSpawner)
        {
            _bulletSpawner = bulletSpawner;
        }
        
        void IGameInitListener.OnInit()
        {
            _bulletSpawner.Init();
            _bulletSpawner.OnBulletSpawned += OnBulletSpawned;
            _bulletSpawner.OnBulletReturned += OnBulletReturnToPool;
            _bulletTracker.OnRequireRemove += OnRequireRemoveBullet;
            DependencyInjector.Inject(_bulletTracker);
        }

        private void OnBulletSpawned(Bullet bullet)
        {
            bullet.OnCollisionEntered += this.OnBulletCollision;
        }

        void IFixedUpdateListener.OnFixedUpdate(float fixedDeltaTime)
        { 
            _bulletTracker.Track(_bulletSpawner.ActiveBullets);
        }

        private void OnBulletCollision(Bullet bullet, Collision2D collision)
        {
            BulletUtils.DealDamage(bullet, collision.gameObject);
            this.RemoveBullet(bullet);
        }

        private void RemoveBullet(Bullet bullet)
        {
            _bulletSpawner.ReturnBullet(bullet);
        }

        private void OnRequireRemoveBullet(Bullet bullet)
        {
            _bulletSpawner.ReturnBullet(bullet);
        }

        private void OnBulletReturnToPool(Bullet bullet)
        {
            bullet.OnCollisionEntered -= this.OnBulletCollision;
        }
    }
}