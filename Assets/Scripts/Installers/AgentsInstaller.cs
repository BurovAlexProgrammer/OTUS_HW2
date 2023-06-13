using Listener;
using Service;
using UnityEngine;

namespace Installers
{
    public class AgentsInstaller : MonoBehaviour
    {
        private IGameListener[] _agents;

        private void Awake()
        {
            _agents = gameObject.GetComponents<IGameListener>();

            InstallServices();
        }

        private void InstallServices()
        {
            foreach (var listener in _agents)
            {
                var t = ServiceLocator.Get<GameSystemsService>();
                t.AddListener(listener);
            }
        }
    }
}