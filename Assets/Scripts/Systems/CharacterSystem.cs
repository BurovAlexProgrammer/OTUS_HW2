using DI;
using GameContext;
using Listener;
using Service;
using ShootEmUp;
using UnityEngine;

namespace Systems
{
    public sealed class CharacterSystem : MonoBehaviour, IGameInitListener, IGameStartListener, IGameOverListener, IFixedUpdateListener
    {
        private GameManager _gameManager;
        private CharacterService _characterService;
        private GameObject _character;
        private InputService _inputService;
        private BulletSpawner _bulletSpawner;
        
        private WeaponComponent _weaponComponent;
        private MoveComponent _moveComponent;

        [Inject]
        public void Construct(
            GameManager gameManager, 
            CharacterService characterService, 
            InputService inputService, 
            BulletSpawner bulletSpawner)
        {
            _gameManager = gameManager;
            _characterService = characterService;
            _character = characterService.Character;
            _inputService = inputService;
            _bulletSpawner = bulletSpawner;
        }

        public void OnInit()
        {
            _moveComponent = _character.GetComponent<MoveComponent>();
            _weaponComponent = _character.GetComponent<WeaponComponent>();
        }
        
        private void Fire()
        {
            var bulletConfig = _characterService.BulletConfig;
            _bulletSpawner.Spawn(new BulletSpawner.Args()
            {
                isPlayer = true,
                physicsLayer = (int) bulletConfig.physicsLayer,
                color = bulletConfig.color,
                damage = bulletConfig.damage,
                position = _weaponComponent.Position,
                velocity = _weaponComponent.Rotation * Vector3.up * bulletConfig.speed
            });
        }

        private void OnCharacterDeath(GameObject _)
        {
            this._gameManager.FinishGame();  
        }

        void IGameStartListener.OnStart()
        {
            this._character.GetComponent<HitPointsComponent>().hpEmpty += OnCharacterDeath;
        }

        void IGameOverListener.OnGameOver()
        {
            this._character.GetComponent<HitPointsComponent>().hpEmpty -= OnCharacterDeath;
        }

        void IFixedUpdateListener.OnFixedUpdate(float fixedDeltaTime)
        {
            if (_inputService.IsFireRequired)
            {
                Fire();
                _inputService.IsFireRequired = false;
            }
            
            _moveComponent.Move(new Vector2(_inputService.HorizontalDirection, 0f) * fixedDeltaTime);
        }
    }
}