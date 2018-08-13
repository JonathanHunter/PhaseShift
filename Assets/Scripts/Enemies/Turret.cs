namespace PhaseShift.Enemies
{
    using UnityEngine;
    using Bullets;

    public class Turret : Enemy
    {
        /// <summary> The rigidbody for this enemy. </summary>
        [SerializeField]
        [Tooltip("The rigidbody for this enemy.")]
        private Rigidbody rgbdy;
        [SerializeField]
        private float shotTime;
        [SerializeField]
        private Transform barrel;
        [SerializeField]
        private GameObject explosion;

        private Transform player;
        private float size;
        private float shotTimer;

        protected override void LocalInitialize()
        {
            this.player = FindObjectOfType<Character.Player>().transform;
        }

        protected override void LocalReInitialize()
        {
            this.size = 1;
            this.transform.localScale = Vector3.one;
            this.shotTimer = this.shotTime;
        }

        protected override void LocalUpdate()
        {
            this.rgbdy.velocity = Vector3.zero;
            if(this.size > 1)
            {
                this.size -= Time.deltaTime;
                this.transform.localScale = Vector3.one * this.size;
            }

            Vector2 towards = this.player.transform.position - this.transform.position;
            this.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, towards));

            if((this.shotTimer -= Time.deltaTime) < 0)
            {
                GameObject b = BulletPool.Instance.GetBullet(BulletPool.BulletTypes.Enemy);
                if (b != null)
                {
                    b.transform.position = this.barrel.position;
                    b.transform.rotation = this.transform.rotation;
                    this.shotTimer = this.shotTime;
                }
            }
        }

        protected override void LocalDeallocate()
        {
        }

        protected override void LocalDelete()
        {
        }

        protected override void TakeDamage()
        {
            this.size = 1.2f;
            hitSound[Random.Range(0, hitSound.Length)].Play();
        }

        protected override void Death()
        {
            Instantiate(explosion, this.transform.position, Quaternion.identity);
            ReturnEnemy();
        }
    }
}
