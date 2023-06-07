using Service;
using UnityEngine;

namespace GameContext
{
    public class GameServicesInstaller : MonoBehaviour
    {
        private ServiceBase[] _services;

        private void Awake()
        {
            _services = gameObject.GetComponentsInChildren<ServiceBase>();

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