namespace PhaseShift.Enemies
{
    using UnityEngine;
    using ObjectPooling;
    using Util;

    /// <summary> Parent class for enemies. </summary>
    public abstract class Enemy : MonoBehaviour, IPoolable
    {
        /// <summary> The type of enemy this is.  (Object pooling management) </summary>
        [SerializeField]
        [Tooltip("The type of enemy this is.  (Object pooling management)")]
        private EnemyPool.EnemyTypes type = EnemyPool.EnemyTypes.Basic;
        [SerializeField]
        private int maxHealth;

        [SerializeField]
        private Collider col;

        /// <summary> The type of enemy this is.  (Object pooling management) </summary>
        public EnemyPool.EnemyTypes Type { get { return this.type; } }

        /// <summary> The index for this enemy in the pool. (Object pooling management) </summary>
        private int referenceIndex = 0;

        private Enums.Layers startLayer;
        private Enums.Layers phaseLayer;
        private bool inTrigger;
        protected int health;

        [SerializeField]
        protected AudioSource[] hitSound;
        [SerializeField]
        protected AudioSource launchSound;

        public void SetLayers(Enums.Layers startLayer, Enums.Layers phaseLayer)
        {
            this.startLayer = startLayer;
            this.phaseLayer = phaseLayer;
            this.gameObject.layer = (int)this.startLayer;
        }

        private void Update()
        {
            if (Managers.GameManager.Instance.IsPaused)
                return;
            if (!this.inTrigger)
                this.gameObject.layer = (int)this.startLayer;

            this.inTrigger = false;
            if (this.health <= 0)
            {
                this.col.enabled = false;
                Death();
            }
            else
                LocalUpdate();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == Enums.Tags.PlayerBullet.ToString())
            {
                this.health--;
                TakeDamage();
            }
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.tag == Enums.Tags.Space.ToString())
            {
                this.gameObject.layer = (int)this.phaseLayer;
                this.inTrigger = true;
            }

            if (collision.gameObject.tag == Enums.Tags.PlayerBullet.ToString())
            {
                this.health--;
                TakeDamage();
            }
        }

        private void OnTriggerStay(Collider collision)
        {
            if (collision.gameObject.tag == Enums.Tags.Space.ToString())
            {
                this.gameObject.layer = (int)this.phaseLayer;
                this.inTrigger = true;
            }
        }

        private void OnTriggerExit(Collider collision)
        {
            if (collision.gameObject.tag == Enums.Tags.Space.ToString())
            {
                this.gameObject.layer = (int)this.startLayer;
                this.inTrigger = true;
            }
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
            this.col.enabled = true;
            this.gameObject.SetActive(true);
            this.inTrigger = false;
            this.health = this.maxHealth;
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

        protected virtual void Death()
        {
            ReturnEnemy();
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
        protected abstract void TakeDamage();
    }
}
