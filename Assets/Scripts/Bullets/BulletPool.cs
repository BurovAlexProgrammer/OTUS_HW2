using System.Collections.Generic;
using UnityEngine;

namespace ShootEmUp
{
    public class BulletPool
    {
        private readonly Queue<Bullet> _bulletPool = new();
        private readonly HashSet<Bullet> _activeBullets = new();
        
        private Bullet _prefab;
        private Transform _container;
        private Transform _worldTransform;

        public HashSet<Bullet> ActiveBullets => _activeBullets;

        public void Init(Bullet prefab, Transform container, int initialCount, Transform worldTransform)
        {
            for (var i = 0; i < initialCount; i++)
            {
                var bullet = GameObject.Instantiate(prefab, container);
                _bulletPool.Enqueue(bullet);
            }
        }

        public Bullet Get()
        {
            if (_bulletPool.TryDequeue(out var bullet))
            {
                bullet.transform.SetParent(_worldTransform);
            }
            else
            {
                bullet = GameObject.Instantiate(_prefab, _worldTransform);
            }
            
            bullet.gameObject.SetActive(true);
            ActiveBullets.Add(bullet);
            return bullet;
        }

        public void Return(Bullet bullet)
        {
            if (ActiveBullets.Remove(bullet))
            {
                bullet.transform.SetParent(_container);
                bullet.gameObject.SetActive(false);
                _bulletPool.Enqueue(bullet);
            }
        }
    }
}