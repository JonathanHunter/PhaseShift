namespace PhaseShift.Bullets
{
    using UnityEngine;
    using ObjectPooling;

    public class EnemyPool : ObjectPool
    {
        /// <summary> The types of bullets to pool. </summary>
        [SerializeField]
        [Tooltip("The types of enemies to pool.")]
        private Enemy[] enemyPools = null;

        /// <summary> How many of each type to pool. </summary>
        [SerializeField]
        [Tooltip("How many of each type to pool.")]
        private int[] enemyPoolSizes = null;

        /// <summary>
        /// The different types of enemies:
        /// Basic: Normal enemy
        /// Turret: Turret enemy
        /// </summary>
        public enum EnemyTypes { Basic, Turret }

        /// <summary> Singleton instance for this object pool. </summary>
        public static EnemyPool Instance { get; private set; }

        protected override void PreInit()
        {
            //if (Instance != null && Instance != this)
            //{
            //    Debug.LogError("Duplicate Bullet Pool detected: removing " + this.gameObject.name);
            //    Destroy(this.gameObject);
            //    return;
            //}

            Instance = this;
        }

        protected override IPoolable[] GetTemplets()
        {
            return this.enemyPools;
        }

        protected override int[] GetPoolSizes()
        {
            return this.enemyPoolSizes;
        }

        /// <summary> Retrieve a enemy from a pool. </summary>
        /// <param name="type"> The type of enemy to get. </param>
        /// <returns> The gameObject of the enemy or null if there are none available. </returns>
        public GameObject GetEnemy(EnemyTypes type)
        {
            IPoolable entity = AllocateEntity(this.enemyPools[(int)type]);
            if (entity == null)
                return null;

            return entity.GetGameObject();
        }

        /// <summary> Returns a enemy to the pool. </summary>
        /// <param name="type"> The type of enemy being returned. </param>
        /// <param name="bullet"> The enemy being returned. </param>
        public void ReturnEnemy(EnemyTypes type, GameObject enemy)
        {
            IPoolable entity = enemy.GetComponent<IPoolable>();
            DeallocateEntity(this.enemyPools[(int)type], entity);
        }
    }
}
