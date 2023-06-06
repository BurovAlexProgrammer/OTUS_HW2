using System.Collections.Generic;
using Listener;
using UnityEngine;

namespace GameContext
{
    public sealed class GameManager : MonoBehaviour
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

            _currentState = State.Finish;
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

            _currentState = State.Init;
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

            _currentState = State.Start;
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

            _currentState = State.GameOver;
        }
    }
}