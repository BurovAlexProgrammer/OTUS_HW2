using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShootEmUp
{
    public class BulletTracker
    {
        public event Action<Bullet> OnRequireRemove;
        
        private readonly List<Bullet> _cache = new();
        private LevelBounds _bounds;

        public void Init(LevelBounds bounds)
        {
            _bounds = bounds;
        }
        
        public void Track(BulletPool bulletPool)
        {
            _cache.Clear();
            _cache.AddRange(bulletPool.ActiveBullets);

            for (int i = 0, count = _cache.Count; i < count; i++)
            {
                var bullet = _cache[i];
                if (!_bounds.InBounds(bullet.transform.position))
                {
                    OnRequireRemove?.Invoke(bullet);
                    // RemoveBullet(bullet);
                }
            }
        }
    }
}