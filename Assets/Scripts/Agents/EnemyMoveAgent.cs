using Listener;
using UnityEngine;

namespace ShootEmUp
{
    public sealed class EnemyMoveAgent : MonoBehaviour, IFixedUpdateListener
    {
        public bool IsReached
        {
            get { return this.isReached; }
        }

        [SerializeField] private MoveComponent moveComponent;

        private Vector2 destination;

        private bool isReached;

        public void SetDestination(Vector2 endPoint)
        {
            this.destination = endPoint;
            this.isReached = false;
        }

        void IFixedUpdateListener.OnFixedUpdate(float fixedDeltaTime)
        {
            if (this.isReached)
            {
                return;
            }
            
            var vector = this.destination - (Vector2) this.transform.position;
            if (vector.magnitude <= 0.25f)
            {
                this.isReached = true;
                return;
            }

            var direction = vector.normalized * fixedDeltaTime;
            this.moveComponent.Move(direction);
        }
    }
}