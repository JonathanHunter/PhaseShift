namespace PhaseShift.Character
{
    using System.Collections.Generic;
    using UnityEngine;
    using Util;

    public class Player : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody rgbdy;
        [SerializeField]
        private GameObject foot;
        [SerializeField]
        private Animator anim;
        [SerializeField]
        private string animatorLayer;
        [SerializeField]
        private float speed;
        [SerializeField]
        private float jumpSpeed;

        private enum State { Idle, Move, Jump, Fall }
        private State currentState;
        private AnimationStateMapper<State> mapper;
        private int inAir;
        private int jump;
        private bool inTrigger;

        private void Start()
        {
            this.currentState = State.Idle;
            int layerIndex = this.anim.GetLayerIndex(this.animatorLayer);
            this.mapper = new AnimationStateMapper<State>(
                new List<string> {
                    this.animatorLayer + ".Idle",
                    this.animatorLayer + ".Move",
                    this.animatorLayer + ".Jump",
                    this.animatorLayer + ".Fall"
                }, 
                this.anim,
                layerIndex);
            this.inAir = Animator.StringToHash("InAir");
            this.jump = Animator.StringToHash("Jump");
            this.inTrigger = false;
        }

        private void Update()
        {
            if (!this.inTrigger)
                this.foot.layer = LayerMask.NameToLayer("Phase");

            this.inTrigger = false;
            this.currentState = this.mapper.GetCurrentState();
            //Debug.Log(this.rgbdy.velocity.y);
            this.anim.SetBool(this.inAir, InAirCheck());
            switch (this.currentState)
            {
                case State.Idle: Idle(); break;
                case State.Move: Move(); break;
                case State.Jump: Jump(); break;
                case State.Fall: Fall(); break;
            }
        }

        private void OnTriggerEnter(Collider collision)
        {
            this.foot.layer = LayerMask.NameToLayer("Ground");
            this.inTrigger = true;
        }

        private void OnTriggerStay(Collider collision)
        {
            this.foot.layer = LayerMask.NameToLayer("Ground");
            this.inTrigger = true;
        }

        private void OnTriggerExit(Collider collision)
        {
            this.foot.layer = LayerMask.NameToLayer("Phase");
        }

        private bool InAirCheck()
        {
            return this.rgbdy.velocity.y < -.5f || this.rgbdy.velocity.y > .5f;
        }

        private void Idle()
        {
        }

        private void Move()
        {
            float xVel = GetXVel();
            this.rgbdy.velocity = new Vector3(xVel, this.rgbdy.velocity.y, 0);
        }

        private void Jump()
        {
            float xVel = GetXVel();
            this.rgbdy.velocity = new Vector3(xVel, this.jumpSpeed, 0);
        }

        private void Fall()
        {
            this.anim.ResetTrigger(this.jump);
            float xVel = GetXVel();
            this.rgbdy.velocity = new Vector3(xVel, this.rgbdy.velocity.y, 0);
        }

        private float GetXVel()
        {
            float xVel = this.rgbdy.velocity.x;
            if (CustomInput.BoolHeld(CustomInput.UserInput.Left))
                xVel = -this.speed;
            else if (CustomInput.BoolHeld(CustomInput.UserInput.Right))
                xVel = this.speed;

            return xVel;
        }
    }
}
