using System.Collections.Generic;
using DI;
using GameContext;
using Service;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Installers
{
    public class Startup : MonoBehaviour
    {
        private GameManager _gameManager;

        private void Awake()
        {
            _gameManager = ServiceLocator.Get<GameManager>();

            _gameManager.InstallListeners();
            ResolveDependencies();
            
            _gameManager.InitGame();
        }

        private void ResolveDependencies()
        {
            var rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
            var transforms = new List<Transform>(rootGameObjects.Length);

            foreach (var rootGameObject in rootGameObjects)
            {
                transforms.AddRange(rootGameObject.GetComponentsInChildren<Transform>());
                transforms.Add(rootGameObject.transform);
            }

            var monobehs = new List<MonoBehaviour>(transforms.Count);
            
            foreach (var child in transforms)
            {
                monobehs.AddRange(child.GetComponents<MonoBehaviour>());
            }
            
            foreach (var monobeh in monobehs)
            {
                DependencyInjector.Inject(monobeh);
            }
        }
    }
}