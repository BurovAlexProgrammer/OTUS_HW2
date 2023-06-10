using UnityEngine;

namespace Service
{
    public class CharacterService : ServiceBase
    {
        [SerializeField] private GameObject _character;
        public GameObject Character => _character;

        public void SetCharacter(GameObject characterGameObjecr)
        {
            _character = characterGameObjecr;
        }
    }
}