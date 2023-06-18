using UnityEngine;

namespace GameContext
{
    public class SceneContext : MonoBehaviour
    {
        [SerializeField] private Transform _worldTransform;

        public Transform WorldTransform => _worldTransform;
    }
}