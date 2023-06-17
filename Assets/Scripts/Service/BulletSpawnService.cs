using System;
using System.Collections.Generic;
using ShootEmUp;
using UnityEngine;

namespace Service
{
    public class BulletSpawnService : ServiceBase
    {
        [SerializeField] private int initialCount = 50;
        [SerializeField] private Transform container;
        [SerializeField] private Bullet prefab;
        [SerializeField] private Transform worldTransform;
        
        public event Action<Bullet> OnBulletSpawned;
        public event Action<Bullet> OnBulletReturned;
        
        private BulletPool _bulletPool;

        public HashSet<Bullet> ActiveBullets => _bulletPool.ActiveBullets;

        public void Init()
        {
            _bulletPool = new BulletPool();
            _bulletPool.Init(prefab,  container, initialCount, worldTransform);
        }
        
        public Bullet Spawn(Args args)
        {
            var bullet = _bulletPool.Get();

            bullet.SetPosition(args.position);
            bullet.SetColor(args.color);
            bullet.SetPhysicsLayer(args.physicsLayer);
            bullet.damage = args.damage;
            bullet.isPlayer = args.isPlayer;
            bullet.SetVelocity(args.velocity);
            OnBulletSpawned?.Invoke(bullet);

            return bullet;
        }

        public void ReturnBullet(Bullet bullet)
        {
            _bulletPool.Return(bullet);
            OnBulletReturned?.Invoke(bullet);
        }
        
        public struct Args
        {
            public Vector2 position;
            public Vector2 velocity;
            public Color color;
            public int physicsLayer;
            public int damage;
            public bool isPlayer;
        }
    }
}