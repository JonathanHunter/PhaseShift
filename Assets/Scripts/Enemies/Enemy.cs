namespace PhaseShift.Bullets
{
    using UnityEngine;
    using ObjectPooling;

    /// <summary> Parent class for enemies. </summary>
    public abstract class Enemy : MonoBehaviour, IPoolable
    {
        /// <summary> The type of enemy this is.  (Object pooling management) </summary>
        [SerializeField]
        [Tooltip("The type of enemy this is.  (Object pooling management)")]
        private EnemyPool.EnemyTypes type = EnemyPool.EnemyTypes.Basic;

        /// <summary> The type of enemy this is.  (Object pooling management) </summary>
        public EnemyPool.EnemyTypes Type { get { return this.type; } }

        /// <summary> The index for this enemy in the pool. (Object pooling management) </summary>
        private int referenceIndex = 0;

        private void Update()
        {
            if (Managers.GameManager.Instance.IsPaused)
            {
                Rigidbody r = this.GetComponent<Rigidbody>();
                if (r != null)
                    r.velocity = Vector3.zero;
                return;
            }

            LocalUpdate();
        }

        private void OnCollisionEnter(Collision collision)
        {
        }

        public IPoolable SpawnCopy(int referenceIndex)
        {
            Enemy enemy = Instantiate<Enemy>(this);
            enemy.referenceIndex = referenceIndex;
            return enemy;
        }

        public GameObject GetGameObject()
        {
            return this.gameObject;
        }

        public int GetReferenceIndex()
        {
            return this.referenceIndex;
        }

        public void Initialize()
        {
            LocalInitialize();
        }

        public void ReInitialize()
        {
            LocalReInitialize();
            this.gameObject.SetActive(true);
        }

        public void Deallocate()
        {
            LocalDeallocate();
            this.gameObject.SetActive(false);
        }

        public void Delete()
        {
            LocalDelete();
            Destroy(this.gameObject);
        }

        protected void ReturnEnemy()
        {
            EnemyPool.Instance.ReturnEnemy(this.type, this.gameObject);
        }

        /// <summary> Local Update for subclasses. </summary>
        protected abstract void LocalUpdate();
        /// <summary> Local Initialize for subclasses. </summary>
        protected abstract void LocalInitialize();
        /// <summary> Local ReInitialize for subclasses. </summary>
        protected abstract void LocalReInitialize();
        /// <summary> Local Deallocate for subclasses. </summary>
        protected abstract void LocalDeallocate();
        /// <summary> Local Delete for subclasses. </summary>
        protected abstract void LocalDelete();
    }
}
