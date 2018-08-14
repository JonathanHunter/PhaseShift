namespace PhaseShift.Managers
{
    using UnityEngine;

    public class Checkpoint : MonoBehaviour
    {

        float speed = 1f;
        public Renderer rend;

        public Color activeColor;

        public GameObject spinObject;

        private bool active;

        private void Start()
        {
            rend = GetComponentInChildren<Renderer>();
            rend.material.shader = Shader.Find("Custom Shaders/BlinderGlow");
        }

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

                active = true;

            }
        }

        private void Update()
        {
            spinObject.transform.Rotate(.5f, 1f, 1.5f);
            if (active)
            {
                activeColor = Color.Lerp(activeColor, rend.material.GetColor("_GlowColor"), .001f);
                rend.material.SetColor("_GlowColor", activeColor);
            }
        }
    }
}
