namespace PhaseShift.Bullets
{
    using UnityEngine;
    using ObjectPooling;
    
    public class BulletPool : ObjectPool
    {
        /// <summary> The types of bullets to pool. </summary>
        [SerializeField]
        [Tooltip("The types of bullets to pool.")]
        private Bullet[] bulletPools = null;

        /// <summary> How many of each type to pool. </summary>
        [SerializeField]
        [Tooltip("How many of each type to pool.")]
        private int[] bulletPoolSizes = null;

        /// <summary>
        /// The different types of bullets:
        /// Player: Pistol bullets from the player
        /// Enemy: Enemy pistol bullets
        /// EnemySpiper: Enemy sniper bullets
        /// </summary>
        public enum BulletTypes { Basic, Space, Enemy}

        /// <summary> Singleton instance for this object pool. </summary>
        public static BulletPool Instance { get; private set; }

        protected override void PreInit()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("Duplicate Bullet Pool detected: removing " + this.gameObject.name);
                Destroy(this.gameObject);
                return;
            }

            Instance = this;
        }

        protected override IPoolable[] GetTemplets()
        {
            return this.bulletPools;
        }

        protected override int[] GetPoolSizes()
        {
            return this.bulletPoolSizes;
        }

        /// <summary> Retrieve a bullet from a pool. </summary>
        /// <param name="type"> The type of bullet to get. </param>
        /// <returns> The gameObject of the bullet or null if there are none available. </returns>
        public GameObject GetBullet(BulletTypes type)
        {
            IPoolable entity = AllocateEntity(this.bulletPools[(int)type]);
            if (entity == null)
                return null;

            return entity.GetGameObject();
        }

        /// <summary> Returns a bullet to the pool. </summary>
        /// <param name="type"> The type of bullet being returned. </param>
        /// <param name="bullet"> The bullet being returned. </param>
        public void ReturnBullet(BulletTypes type, GameObject bullet)
        {
            IPoolable entity = bullet.GetComponent<IPoolable>();
            DeallocateEntity(this.bulletPools[(int)type], entity);
        }
    }
}
