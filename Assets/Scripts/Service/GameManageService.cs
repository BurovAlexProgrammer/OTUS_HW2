using System.Collections.Generic;
using Listener;
using UnityEngine;

namespace Service
{
    public class GameManageService : ServiceBase
    {
        private List<IGameListener> _gameListeners = new List<IGameListener>();
        private State _currentState = State.Undefined;
        
        public enum State
        {
            Undefined,
            Init,
            Start,
            GameOver,
            Finish
        }

        public void AddListener(IGameListener listener)
        {
            _gameListeners.Add(listener);
        }

        public void FinishGame()
        {
            Debug.Log("Game over!");
            Time.timeScale = 0;
            
            foreach (var gameListener in _gameListeners)
            {
                if (gameListener is IGameOverListener listener)
                {
                    listener.OnGameOver();
                }
            }

            SetState(State.Finish);
        }

        public void InitGame()
        {
            foreach (var gameListener in _gameListeners)
            {
                if (gameListener is IGameInitListener listener)
                {
                    listener.OnInit();
                }
            }

            SetState(State.Init);
        }

        public void StartGame()
        {
            foreach (var gameListener in _gameListeners)
            {
                if (gameListener is IGameStartListener listener)
                {
                    listener.OnStart();
                }
            }

            SetState(State.Start);
        }
        
        public void GameOver()
        {
            foreach (var gameListener in _gameListeners)
            {
                if (gameListener is IGameOverListener listener)
                {
                    listener.OnGameOver();
                }
            }

            SetState(State.GameOver);
        }

        private void SetState(State state)
        {
            _currentState = state;
            Debug.Log("GameState: " + state);
        }
    }
}