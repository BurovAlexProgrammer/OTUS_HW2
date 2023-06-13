using Installers;
using Listener;
using UnityEngine;

namespace Service
{
    public class GameSystemsService : ServiceBase
    {
        private GameSystemsInstaller _gameSystemsInstaller;
        
        public void Init(GameSystemsInstaller gameSystemsInstaller)
        {
            _gameSystemsInstaller = gameSystemsInstaller;
        }

        public void AddListener(IGameListener gameListener)
        {
            if (gameListener is IFixedUpdateListener fixedUpdateListener)
            {
                _gameSystemsInstaller.FixedUpdateListeners.Add(fixedUpdateListener);
                return;
            }
            
            if (gameListener is IUpdateListener updateListener)
            {
                _gameSystemsInstaller.UpdateListeners.Add(updateListener);
                return;
            }
            
            _gameSystemsInstaller.Listeners.Add(gameListener);
        }
    }
}