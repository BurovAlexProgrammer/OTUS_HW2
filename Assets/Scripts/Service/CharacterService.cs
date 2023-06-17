using ShootEmUp;
using UnityEngine;

namespace Service
{
    public class CharacterService : ServiceBase
    {
        [SerializeField] private GameObject _character;
        [SerializeField] private BulletConfig _bulletConfig;
        
        public GameObject Character => _character;
        public BulletConfig BulletConfig => _bulletConfig;

        public void SetCharacter(GameObject characterGameObjecr)
        {
            _character = characterGameObjecr;
        }
    }
}