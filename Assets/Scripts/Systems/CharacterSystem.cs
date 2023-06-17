using DI;
using Listener;
using Service;
using ShootEmUp;
using UnityEngine;

namespace Systems
{
    public sealed class CharacterSystem : MonoBehaviour, IGameInitListener, IGameStartListener, IGameOverListener, IFixedUpdateListener
    {
        private GameManageService _gameManageService;
        private CharacterService _characterService;
        private GameObject _character;
        private InputService _inputService;
        private BulletSpawnService _bulletSpawnService;
        
        private WeaponComponent _weaponComponent;
        private MoveComponent _moveComponent;

        [Inject]
        public void Construct(GameManageService gameManageService, CharacterService characterService, InputService inputService, BulletSpawnService bulletSpawnService)
        {
            _gameManageService = gameManageService;
            _characterService = characterService;
            _character = characterService.Character;
            _inputService = inputService;
            _bulletSpawnService = bulletSpawnService;
        }

        public void OnInit()
        {
            _moveComponent = _character.GetComponent<MoveComponent>();
            _weaponComponent = _character.GetComponent<WeaponComponent>();
        }
        
        private void OnFlyBullet()
        {
            var bulletConfig = _characterService.BulletConfig;
            _bulletSpawnService.Spawn(new BulletSpawnService.Args()
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
            this._gameManageService.FinishGame();  
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
                this.OnFlyBullet();
                _inputService.IsFireRequired = false;
            }
            
            _moveComponent.Move(new Vector2(_inputService.HorizontalDirection, 0f) * fixedDeltaTime);
        }
    }
}