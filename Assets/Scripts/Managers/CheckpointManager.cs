namespace PhaseShift.Managers
{
    using UnityEngine;

    public class CheckpointManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] checkpoints;
        [SerializeField]
        private Character.Player player;

        public static CheckpointManager Instance { get { return instance; } }

        private static CheckpointManager instance;

        private int currentCheckpoint;

        private void Start()
        {
            instance = this;
            this.currentCheckpoint = 0;
        }

        public void Respawn()
        {
            this.player.transform.position = this.checkpoints[this.currentCheckpoint].transform.position;
        }

        public void RegisterCheckpoint(int number)
        {
            if (number > this.currentCheckpoint)
                this.currentCheckpoint = number;
        }
    }
}
