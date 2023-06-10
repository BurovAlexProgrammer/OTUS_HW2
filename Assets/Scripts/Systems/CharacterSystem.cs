using DI;
using Listener;
using Service;
using UnityEngine;

namespace ShootEmUp
{
    public sealed class CharacterSystem : MonoBehaviour, IGameStartListener, IGameOverListener, IFixedUpdateListener
    {
        [SerializeField] private BulletSystem _bulletSystem;
        [SerializeField] private BulletConfig _bulletConfig;
        
        public bool _fireRequired;
        private GameManageService _gameManageService;
        private GameObject _character; 

        [Inject]
        public void Construct(GameManageService gameManageService, CharacterService characterService)
        {
            _gameManageService = gameManageService;
            _character = characterService.Character;
        }

        private void OnFlyBullet()
        {
            var weapon = this._character.GetComponent<WeaponComponent>();
            _bulletSystem.FlyBulletByArgs(new BulletSystem.Args
            {
                isPlayer = true,
                physicsLayer = (int) this._bulletConfig.physicsLayer,
                color = this._bulletConfig.color,
                damage = this._bulletConfig.damage,
                position = weapon.Position,
                velocity = weapon.Rotation * Vector3.up * this._bulletConfig.speed
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
            if (this._fireRequired)
            {
                this.OnFlyBullet();
                this._fireRequired = false;
            }
        }
    }
}