namespace PhaseShift.Enemies
{
    using UnityEngine;

    public class BasicEnemy : Enemy
    {
        /// <summary> How fast this enemy moves. </summary>
        [SerializeField]
        [Tooltip("How fast this enemy moves.")]
        private float speed;

        /// <summary> The rigidbody for this enemy. </summary>
        [SerializeField]
        [Tooltip("The rigidbody for this enemy.")]
        private Rigidbody rgbdy;

        protected override void LocalInitialize()
        {
        }

        protected override void LocalReInitialize()
        {
        }

        protected override void LocalUpdate()
        {
            throw new System.NotImplementedException();
        }

        protected override void LocalDeallocate()
        {
            this.rgbdy.velocity = Vector3.zero;
        }

        protected override void LocalDelete()
        {
        }
    }
}
