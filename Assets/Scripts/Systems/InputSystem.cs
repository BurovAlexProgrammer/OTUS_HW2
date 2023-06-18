using DI;
using Listener;
using Service;
using ShootEmUp;
using UnityEngine;

namespace Systems
{
    public sealed class InputSystem : MonoBehaviour, IUpdateListener
    {
        private InputService _inputService;

        [Inject]
        public void Construct(InputService inputService)
        {
            _inputService = inputService;
        }

        public void OnUpdate(float deltaTime)
        {
            _inputService.IsFireRequired = Input.GetKeyDown(KeyCode.Space);

            _inputService.HorizontalDirection =
                Input.GetKey(KeyCode.LeftArrow) ? -1
                : Input.GetKey(KeyCode.RightArrow) ? 1
                : 0;
        }
    }
}