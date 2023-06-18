using Enemy;
using GameContext;
using Service;
using ShootEmUp;
using Systems;
using UnityEngine;

namespace Installers
{
    public class ServiceLocatorInstaller : MonoBehaviour
    {
        private MonoBehaviour[] _services;

        private void Awake()
        {
            _services = gameObject.GetComponentsInChildren<MonoBehaviour>();
            var sceneContext = Utils.FindComponentInActiveScene<SceneContext>();
            var levelBounds = Utils.FindComponentInActiveScene<LevelBounds>();
            var gameManager = Utils.FindComponentInActiveScene<GameManager>();
            var inputService = new InputService();
            ServiceLocator.Add(inputService);
            ServiceLocator.Add(levelBounds);
            InstallServices();
        }

        private void InstallServices()
        {
            foreach (var service in _services)
            {
                ServiceLocator.Add(service);
            }
        }
    }
}