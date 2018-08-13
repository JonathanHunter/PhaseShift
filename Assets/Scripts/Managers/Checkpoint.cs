namespace PhaseShift.Managers
{
    using UnityEngine;

    public class Checkpoint : MonoBehaviour
    {
        [SerializeField]
        private int number;

        [SerializeField]
        private Collider col;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == Util.Enums.Tags.Player.ToString())
            {
                CheckpointManager.Instance.RegisterCheckpoint(this.number);
                this.col.enabled = false;
            }
        }
    }
}
