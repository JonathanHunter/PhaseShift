namespace PhaseShift.Character
{
    using UnityEngine;
    using Util;

    public class Controller : MonoBehaviour
    {
        [SerializeField]
        private Animator anim;

        private int move;
        private int jump;

        private void Start()
        {
            this.move = Animator.StringToHash("Move");
            this.jump = Animator.StringToHash("Jump");
        }

        private void Update()
        {
            this.anim.SetBool(this.move, (CustomInput.BoolHeld(CustomInput.UserInput.Left) || CustomInput.BoolHeld(CustomInput.UserInput.Right)));
            if (CustomInput.BoolFreshPress(CustomInput.UserInput.Jump))
                this.anim.SetTrigger(this.jump);
        }
    }
}
