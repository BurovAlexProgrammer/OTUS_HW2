using Listener;
using Service;
using UnityEngine;

namespace GameContext
{
    public class GameInstaller : MonoBehaviour
    {
        [SerializeField] private ServiceBase[] _services;

        private IGameListener[] _listeners;
        private IUpdateListener[] _updateListeners;
        private GameManager _gameManager;

        private void Awake()
        {
            _gameManager = GetComponent<GameManager>();
            _listeners = GetComponentsInChildren<IGameListener>();
            _updateListeners = GetComponentsInChildren<IUpdateListener>();

            InstallServices();
            InstallListeners();
            ResolveDependencies();
            _gameManager.InitGame();
        }

        private void InstallServices()
        {
            foreach (var service in _services)
            {
                ServiceLocator.Add(service);
            }
        }

        private void InstallListeners()
        {
            foreach (var gameListener in _listeners)
            {
                _gameManager.AddListener(gameListener);
            }
        }

        private void ResolveDependencies()
        {
        }

        private void Update()
        {
            foreach (var updateListener in _updateListeners)
            {
                updateListener.OnUpdate();
            }
        }
    }
}