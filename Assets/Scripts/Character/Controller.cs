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
        private int shoot1;
        private int shoot2;

        private void Start()
        {
            this.move = Animator.StringToHash("Move");
            this.jump = Animator.StringToHash("Jump");
            this.shoot1 = Animator.StringToHash("Shoot1");
            this.shoot2 = Animator.StringToHash("Shoot2");
        }

        private void Update()
        {
            this.anim.SetBool(this.move, (CustomInput.BoolHeld(CustomInput.UserInput.Left) || CustomInput.BoolHeld(CustomInput.UserInput.Right)));
            if (CustomInput.BoolFreshPress(CustomInput.UserInput.Jump))
                this.anim.SetTrigger(this.jump);

            if (CustomInput.BoolFreshPress(CustomInput.UserInput.Shoot1))
                this.anim.SetTrigger(this.shoot1);

            if (CustomInput.BoolFreshPress(CustomInput.UserInput.Shoot2))
                this.anim.SetTrigger(this.shoot2);
        }
    }
}
