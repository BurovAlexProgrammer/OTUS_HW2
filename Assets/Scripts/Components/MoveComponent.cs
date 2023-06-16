using UnityEngine;

namespace ShootEmUp
{
    public sealed class MoveComponent : MonoBehaviour
    {
        [SerializeField]
        private new Rigidbody2D rigidbody2D;

        [SerializeField]
        private float speed = 5.0f;
        
        public void Move(Vector2 velocity)
        {
            var nextPosition = this.rigidbody2D.position + velocity * this.speed;
            this.rigidbody2D.MovePosition(nextPosition);
        }
    }
}