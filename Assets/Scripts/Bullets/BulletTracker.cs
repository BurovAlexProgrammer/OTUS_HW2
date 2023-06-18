using System;
using System.Collections.Generic;
using DI;
using UnityEngine;

namespace ShootEmUp
{
    public class BulletTracker
    {
        public event Action<Bullet> OnRequireRemove;
        
        private readonly List<Bullet> _cache = new();
        private LevelBounds _bounds;

        [Inject]
        public void Construct(LevelBounds bounds)
        {
            _bounds = bounds;
        }
        
        public void Track(HashSet<Bullet> activeBullets)
        {
            _cache.Clear();
            _cache.AddRange(activeBullets);

            for (int i = 0, count = _cache.Count; i < count; i++)
            {
                var bullet = _cache[i];
                if (!_bounds.InBounds(bullet.transform.position))
                {
                    OnRequireRemove?.Invoke(bullet);
                }
            }
        }
    }
}