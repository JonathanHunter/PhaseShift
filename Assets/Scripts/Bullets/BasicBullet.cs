namespace PhaseShift.Bullets
{
    using UnityEngine;

    /// <summary> Handler for basic bullets. </summary>
    public class BasicBullet : Bullet
    {
        /// <summary> How fast this bullet travels. </summary>
        [SerializeField]
        [Tooltip("How fast this bullet travels.")]
        private float speed;

        /// <summary> The rigidbody for this bullet. </summary>
        [SerializeField]
        [Tooltip("The rigidbody for this bullet.")]
        private Rigidbody rgbdy;

        protected override void LocalInitialize()
        {
        }

        protected override void LocalReInitialize()
        {
        }

        protected override void LocalUpdate()
        {
            this.rgbdy.velocity = this.transform.forward * speed;
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
