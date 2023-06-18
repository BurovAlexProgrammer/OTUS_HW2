using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Utils
    {
        public static T FindComponentInActiveScene<T>() 
        {
            var gameObjects = SceneManager.GetActiveScene().GetRootGameObjects();

            foreach (var gameObject in gameObjects)
            {
                var result = gameObject.GetComponent<T>();
                var childrenResult = gameObject.GetComponentInChildren<T>();
                
                if (result != null) return result;
                if (childrenResult != null) return childrenResult;
            }

            return default;
        }
        
        public static IEnumerable<T> FindComponentsInActiveScene<T>()
        {
            var gameObjects = SceneManager.GetActiveScene().GetRootGameObjects();

            foreach (var gameObject in gameObjects)
            {
                var mainComponents = gameObject.GetComponents<T>();
                var childComponents = gameObject.GetComponentsInChildren<T>();
                
                foreach (var mainComponent in mainComponents)
                {
                    yield return mainComponent;
                }
                
                foreach (var childComponent in childComponents)
                {
                    yield return childComponent;
                }
            }
        }
    }
