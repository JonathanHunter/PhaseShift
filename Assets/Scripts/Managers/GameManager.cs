namespace PhaseShift.Managers
{
    using UnityEngine;
    using ObjectPooling;

    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private ObjectPool[] pools;

        public enum GameState { Playing, Paused}

        public static GameManager Instance { get { return instance; } }

        public GameState CurrentState { get { return this.currentState; } set { this.currentState = value; } }

        public bool IsPaused { get { return this.currentState != GameState.Playing; } }

        private GameState currentState;
        private static GameManager instance;

        private void Start()
        {
            instance = this;
            this.currentState = GameState.Playing;
            foreach (ObjectPool pool in this.pools)
                pool.Init();
        }
    }
}
