using System.Collections.Generic;
using System.Linq;
using Listener;
using UnityEngine;

namespace GameContext
{
    public class GameManager : MonoBehaviour
    {
        private List<IGameListener> _gameListeners = new List<IGameListener>();
        private List<IUpdateListener> _updateListeners = new List<IUpdateListener>();
        private List<IFixedUpdateListener> _fixedUpdateListeners = new List<IFixedUpdateListener>();
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
            if (listener is IFixedUpdateListener fixedUpdateListener)
            {
                _fixedUpdateListeners.Add(fixedUpdateListener);
                return;
            }
            
            if (listener is IUpdateListener updateListener)
            {
                _updateListeners.Add(updateListener);
                return;
            }
            
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

        public void InstallListeners()
        {
            _gameListeners = Utils.FindComponentsInActiveScene<IGameListener>().ToList();
            _updateListeners = Utils.FindComponentsInActiveScene<IUpdateListener>().ToList();
            _fixedUpdateListeners = Utils.FindComponentsInActiveScene<IFixedUpdateListener>().ToList();
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


        private void SetState(State state)
        {
            _currentState = state;
            Debug.Log("GameState: " + state);
        }
    }
}