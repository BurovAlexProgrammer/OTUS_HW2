using DI;
using Listener;
using Service;
using UnityEngine;

namespace ShootEmUp
{
    public sealed class CharacterSystem : MonoBehaviour, IGameInitListener, IGameStartListener, IGameOverListener, IFixedUpdateListener
    {
        [SerializeField] private BulletSystem _bulletSystem;
        [SerializeField] private BulletConfig _bulletConfig;
        
        private GameManageService _gameManageService;
        private GameObject _character;
        private InputService _inputService;
        
        private WeaponComponent _weaponComponent;
        private MoveComponent _moveComponent;

        [Inject]
        public void Construct(GameManageService gameManageService, CharacterService characterService, InputService inputService)
        {
            _gameManageService = gameManageService;
            _character = characterService.Character;
            _inputService = inputService;
        }

        public void OnInit()
        {
            _moveComponent = _character.GetComponent<MoveComponent>();
            _weaponComponent = _character.GetComponent<WeaponComponent>();
        }
        
        private void OnFlyBullet()
        {
            _bulletSystem.FlyBulletByArgs(new BulletSystem.Args
            {
                isPlayer = true,
                physicsLayer = (int) this._bulletConfig.physicsLayer,
                color = this._bulletConfig.color,
                damage = this._bulletConfig.damage,
                position = _weaponComponent.Position,
                velocity = _weaponComponent.Rotation * Vector3.up * this._bulletConfig.speed
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
            
            _moveComponent.MoveByRigidbodyVelocity(new Vector2(_inputService.HorizontalDirection, 0f) * fixedDeltaTime);
        }
    }
}