namespace PhaseShift.Bullets
{
    using UnityEngine;
    using ObjectPooling;

    /// <summary> Parent class for bullets. </summary>
    public abstract class Bullet : MonoBehaviour, IPoolable
    {
        /// <summary> How long this bullet can exist untouched in the scene. </summary>
        [SerializeField]
        [Tooltip("How long this bullet can exist untouched in the scene.")]
        private float lifeTime = 1;

        /// <summary> The type of bullet this is.  (Object pooling management) </summary>
        [SerializeField]
        [Tooltip("The type of bullet this is.  (Object pooling management)")]
        private BulletPool.BulletTypes type = BulletPool.BulletTypes.Basic;

        /// <summary> The type of bullet this is.  (Object pooling management) </summary>
        public BulletPool.BulletTypes Type { get { return this.type; } }

        /// <summary> The index for this bullet in the pool. (Object pooling management) </summary>
        private int referenceIndex = 0;

        /// <summary> Tracks the remaing time for the bullet to exist in the scene. </summary>
        protected float currentLifeTime = 0;

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
            if ((this.currentLifeTime -= Time.deltaTime) <= 0)
            {
                ReturnBullet();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            this.currentLifeTime = 0;
        }

        public IPoolable SpawnCopy(int referenceIndex)
        {
            Bullet bullet = Instantiate<Bullet>(this);
            bullet.referenceIndex = referenceIndex;
            return bullet;
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
            this.currentLifeTime = this.lifeTime;
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

        protected void ReturnBullet()
        {
            BulletPool.Instance.ReturnBullet(this.type, this.gameObject);
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
