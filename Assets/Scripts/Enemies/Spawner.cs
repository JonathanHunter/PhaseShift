namespace PhaseShift.Enemies
{
    using UnityEngine;

    public class Spawner : MonoBehaviour
    {
        [SerializeField]
        private Transform left;
        [SerializeField]
        private Transform right;
        [SerializeField]
        private EnemyPool.EnemyTypes type;
        [SerializeField]
        private Util.Enums.Layers startLayer;
        [SerializeField]
        private Util.Enums.Layers phaseLayer;

        private GameObject player;
        private GameObject enemy;
        private bool inside;

        private void Start()
        {
            player = FindObjectOfType<Character.Player>().gameObject;
        }

        private void Update()
        {
            if(!inside)
            {
                if(this.player.transform.position.x > this.left.position.x && this.player.transform.position.x < this.right.position.x)
                {
                    this.enemy = EnemyPool.Instance.GetEnemy(this.type);
                    if (this.enemy != null)
                    {
                        this.enemy.transform.position = this.transform.position;
                        this.enemy.GetComponent<Enemy>().SetLayers(this.startLayer, this.phaseLayer);
                    }

                    this.inside = true;
                }
            }
            else
            {
                if (this.player.transform.position.x < this.left.position.x || this.player.transform.position.x > this.right.position.x)
                {
                    if (this.enemy != null && this.enemy.activeSelf)
                    {
                        EnemyPool.Instance.ReturnEnemy(this.type, this.enemy);
                        this.enemy = null;
                    }

                    this.inside = false;
                }
            }
        }
    }
}
