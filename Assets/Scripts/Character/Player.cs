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
        private Collider platformCollider;
        [SerializeField]
        private Animator anim;
        [SerializeField]
        private int layerIndex;
        [SerializeField]
        private float speed;
        [SerializeField]
        private float jumpSpeed;

        private enum State { Idle, Move, Jump, Fall }
        private State currentState;
        private AnimationStateMappper<State> mapper;

        private void Start()
        {
            this.currentState = State.Idle;
            this.mapper = new AnimationStateMappper<State>(new List<string>() { "", "", "" }, this.anim, this.layerIndex);
            this.anim.SetBool("InAir", InAirCheck());
            switch(this.currentState)
            {
                case State.Idle: Idle(); break;
                case State.Move: Move(); break;
                case State.Jump: Jump(); break;
                case State.Fall: Fall(); break;
            }
        }

        private void Update()
        {
            this.currentState = this.mapper.GetCurrentState();
        }

        private void OnTriggerEnter(Collider collision)
        {
            this.platformCollider.enabled = true;
        }

        private void OnTriggerStay(Collider collision)
        {
            this.platformCollider.enabled = true;
        }

        private void OnTriggerExit(Collider collision)
        {
            this.platformCollider.enabled = false;
        }

        private bool InAirCheck()
        {
            return this.rgbdy.velocity.y < .5f || this.rgbdy.velocity.y > .5f;
        }

        private void Idle()
        {
        }

        private void Move()
        {
            this.rgbdy.velocity = Vector2.zero;
            float xVel = GetXVel();
            this.rgbdy.velocity = new Vector3(xVel, this.rgbdy.velocity.y, 0);
        }

        private void Jump()
        {
            this.rgbdy.velocity = Vector2.zero;
            float xVel = GetXVel();
            this.rgbdy.velocity = new Vector3(xVel, this.jumpSpeed, 0);
        }

        private void Fall()
        {
            this.rgbdy.velocity = Vector2.zero;
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
