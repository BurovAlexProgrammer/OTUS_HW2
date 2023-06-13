using System.Collections.Generic;
using System.Linq;
using DI;
using Listener;
using Service;
using UnityEngine;

namespace Installers
{
    public class GameSystemsInstaller : MonoBehaviour
    {
        [SerializeField] private List<MonoBehaviour> _dependencies;

        public List<IGameListener> Listeners;
        public List<IUpdateListener> UpdateListeners;
        public List<IFixedUpdateListener> FixedUpdateListeners;
        private GameManageService _gameManageService;

        private void Awake()
        {
            ServiceLocator.Get<GameSystemsService>().Init(this);
            _gameManageService = ServiceLocator.Get<GameManageService>();
            Listeners = GetComponentsInChildren<IGameListener>().ToList();
            UpdateListeners = GetComponentsInChildren<IUpdateListener>().ToList();
            FixedUpdateListeners = GetComponentsInChildren<IFixedUpdateListener>().ToList();

            InstallListeners();
            ResolveDependencies();
            
            _gameManageService.InitGame();
        }

        private void InstallListeners()
        {
            foreach (var gameListener in Listeners)
            {
                _gameManageService.AddListener(gameListener);
            }
        }

        private void ResolveDependencies()
        {
            foreach (var dependency in _dependencies)
            {
                DependencyInjector.Inject(dependency);
            }
        }

        private void Update()
        {
            foreach (var updateListener in UpdateListeners)
            {
                updateListener.OnUpdate(Time.deltaTime);
            }
        }

        private void FixedUpdate()
        {
            foreach (var fixedUpdateListener in FixedUpdateListeners)
            {
                fixedUpdateListener.OnFixedUpdate(Time.fixedDeltaTime);
            }
        }
    }
}