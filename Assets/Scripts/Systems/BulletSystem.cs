using DI;
using Listener;
using Service;
using UnityEngine;

namespace ShootEmUp
{
    public sealed class BulletSystem : MonoBehaviour, IFixedUpdateListener, IGameInitListener
    {
        [SerializeField] private LevelBounds levelBounds;

        private BulletSpawnService _bulletSpawnService;
        private BulletTracker _bulletTracker = new BulletTracker();

        [Inject]
        public void Construct(BulletSpawnService bulletSpawnService)
        {
            _bulletSpawnService = bulletSpawnService;
        }
        
        void IGameInitListener.OnInit()
        {
            _bulletSpawnService.Init();
            _bulletSpawnService.OnBulletSpawned += OnBulletSpawned;
            _bulletSpawnService.OnBulletReturned += OnBulletReturnToPool;
            _bulletTracker.Init(levelBounds);
            _bulletTracker.OnRequireRemove += OnRequireRemoveBullet;
        }

        private void OnBulletSpawned(Bullet bullet)
        {
            bullet.OnCollisionEntered += this.OnBulletCollision;
        }

        void IFixedUpdateListener.OnFixedUpdate(float fixedDeltaTime)
        { 
            _bulletTracker.Track(_bulletSpawnService.ActiveBullets);
        }

        private void OnBulletCollision(Bullet bullet, Collision2D collision)
        {
            BulletUtils.DealDamage(bullet, collision.gameObject);
            this.RemoveBullet(bullet);
        }

        private void RemoveBullet(Bullet bullet)
        {
            _bulletSpawnService.ReturnBullet(bullet);
        }

        private void OnRequireRemoveBullet(Bullet bullet)
        {
            _bulletSpawnService.ReturnBullet(bullet);
        }

        private void OnBulletReturnToPool(Bullet bullet)
        {
            bullet.OnCollisionEntered -= this.OnBulletCollision;
        }
    }
}