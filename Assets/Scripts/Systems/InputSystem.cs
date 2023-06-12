using DI;
using Listener;
using Service;
using ShootEmUp;
using UnityEngine;

namespace Systems
{
    public sealed class InputSystem : MonoBehaviour, IUpdateListener
    {
        [SerializeField] private GameObject character;

        [SerializeField] private CharacterSystem _characterSystem;

        private InputService _inputService;

        [Inject]
        public void Construct(InputService inputService)
        {
            _inputService = inputService;
        }

        public void OnUpdate(float deltaTime)
        {
            _inputService.IsFireRequired = Input.GetKeyDown(KeyCode.Space);
            // _characterSystem._fireRequired = true;

            _inputService.HorizontalDirection =
                Input.GetKey(KeyCode.LeftArrow) ? -1
                : Input.GetKey(KeyCode.RightArrow) ? 1
                : 0;
        }

        // public void OnFixedUpdate(float fixedDeltaTime)
        // {
        //     this.character.GetComponent<MoveComponent>().MoveByRigidbodyVelocity(new Vector2(this.HorizontalDirection, 0) * Time.fixedDeltaTime);
        // }
    }
}