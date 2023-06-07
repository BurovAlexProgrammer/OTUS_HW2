using DI;
using Listener;
using Service;
using UnityEngine;

namespace ShootEmUp
{
    public sealed class CharacterControlSystem : MonoBehaviour, IGameStartListener, IGameOverListener, IFixedUpdateListener
    {
        [SerializeField] private GameObject character; 
        [SerializeField] private BulletSystem _bulletSystem;
        [SerializeField] private BulletConfig _bulletConfig;
        
        public bool _fireRequired;
        private GameManageService _gameManageService;

        [Inject]
        public void Construct(GameManageService gameManageService)
        {
            _gameManageService = gameManageService;
        }

        private void OnFlyBullet()
        {
            var weapon = this.character.GetComponent<WeaponComponent>();
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
            this.character.GetComponent<HitPointsComponent>().hpEmpty += OnCharacterDeath;
        }

        void IGameOverListener.OnGameOver()
        {
            this.character.GetComponent<HitPointsComponent>().hpEmpty -= OnCharacterDeath;
        }

        void IFixedUpdateListener.OnFixedUpdate()
        {
            if (this._fireRequired)
            {
                this.OnFlyBullet();
                this._fireRequired = false;
            }
        }
    }
}