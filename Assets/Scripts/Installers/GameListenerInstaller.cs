using GameContext;
using Listener;
using Service;
using UnityEngine;

namespace Installers
{
    public class GameListenerInstaller : MonoBehaviour
    {
        private IGameListener[] _listeners;

        private void Awake()
        {
            _listeners = gameObject.GetComponents<IGameListener>();

            InstallServices();
        }

        private void InstallServices()
        {
            foreach (var listener in _listeners)
            {
                var gameManager = ServiceLocator.Get<GameManager>();
                gameManager.AddListener(listener);
            }
        }
    }
}