namespace PhaseShift.Managers
{
    using UnityEngine;

    public class Checkpoint : MonoBehaviour
    {
        [SerializeField]
        private int number;

        [SerializeField]
        private Collider col;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == Util.Enums.Tags.Player.ToString())
            {
                CheckpointManager.Instance.RegisterCheckpoint(this.number);
                this.col.enabled = false;
            }
        }
    }
}
