using UnityEngine;

namespace ShootEmUp
{
    public class BulletSpawner
    {
        private BulletPool _bulletPool;
        
        public void Init(BulletPool bulletPool)
        {
            _bulletPool = bulletPool;
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

            return bullet;
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