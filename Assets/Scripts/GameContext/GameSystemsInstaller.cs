using System;
using System.Collections.Generic;
using DI;
using Listener;
using Service;
using UnityEngine;

namespace GameContext
{
    public class GameSystemsInstaller : MonoBehaviour
    {
        [SerializeField] private List<MonoBehaviour> _dependencies;

        private IGameListener[] _listeners;
        private IUpdateListener[] _updateListeners;
        private IFixedUpdateListener[] _fixedUpdateListeners;
        private GameManageService _gameManageService;

        private void Awake()
        {
            _gameManageService = ServiceLocator.Get<GameManageService>();
            _listeners = GetComponentsInChildren<IGameListener>();
            _updateListeners = GetComponentsInChildren<IUpdateListener>();
            _fixedUpdateListeners = GetComponentsInChildren<IFixedUpdateListener>();

            InstallListeners();
            ResolveDependencies();
            
            _gameManageService.InitGame();
        }

        private void InstallListeners()
        {
            foreach (var gameListener in _listeners)
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
            foreach (var updateListener in _updateListeners)
            {
                updateListener.OnUpdate(Time.deltaTime);
            }
        }

        private void FixedUpdate()
        {
            foreach (var fixedUpdateListener in _fixedUpdateListeners)
            {
                fixedUpdateListener.OnFixedUpdate(Time.fixedDeltaTime);
            }
        }
    }
}