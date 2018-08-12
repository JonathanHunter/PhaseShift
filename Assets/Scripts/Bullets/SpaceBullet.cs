namespace PhaseShift.Bullets
{
    using UnityEngine;

    /// <summary> Handler for basic bullets. </summary>
    public class SpaceBullet : Bullet
    {
        /// <summary> How fast this bullet travels. </summary>
        [SerializeField]
        [Tooltip("How fast this bullet travels.")]
        private float speed;

        /// <summary> The rigidbody for this bullet. </summary>
        [SerializeField]
        [Tooltip("The rigidbody for this bullet.")]
        private Rigidbody rgbdy;
        [SerializeField]
        private float detonationSize;
        [SerializeField]
        private float shrinkSpeed;
        [SerializeField]
        private GameObject ball;

        private bool isDetonated;
        private bool doOnce;
        private float currentSize;
        private float originalSize;

        public void Detonate()
        {
            this.isDetonated = true;
            this.transform.localScale = Vector3.one * this.detonationSize;
            this.currentSize = this.detonationSize;
            this.rgbdy.velocity = Vector3.zero;
            this.rgbdy.useGravity = false;
            this.ball.SetActive(false);
        }

        protected override void LocalInitialize()
        {
            this.originalSize = this.transform.localScale.x;
        }

        protected override void LocalReInitialize()
        {
            this.transform.localScale = Vector3.one * this.originalSize;
            this.rgbdy.useGravity = true;
            this.ball.SetActive(true);
            this.doOnce = false;
        }

        protected override void LocalUpdate()
        {
            if(!this.doOnce)
            {
                this.rgbdy.velocity = this.transform.right * this.speed;
                this.doOnce = true;
            }

            if (this.isDetonated)
            {
                this.currentLifeTime = 99f;
                this.currentSize -= Time.deltaTime * this.shrinkSpeed;
                if (this.currentSize <= 0)
                    ReturnBullet();
                else
                    this.transform.localScale = Vector3.one * this.currentSize;
            }
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
