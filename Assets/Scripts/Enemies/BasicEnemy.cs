namespace PhaseShift.Enemies
{
    using System.Collections.Generic;
    using UnityEngine;
    using Util;

    public class BasicEnemy : Enemy
    {
        /// <summary> How fast this enemy moves. </summary>
        [SerializeField]
        [Tooltip("How fast this enemy moves.")]
        private float speed;
        /// <summary> The rigidbody for this enemy. </summary>
        [SerializeField]
        [Tooltip("The rigidbody for this enemy.")]
        private Rigidbody rgbdy;
        [SerializeField]
        private Animator anim;
        [SerializeField]
        private string animatorLayer;
        [SerializeField]
        private Transform leftBoundary;
        [SerializeField]
        private Transform rightBoundary;

        private enum State { Idle, Turn, Hurt, Death }

        private State currentState;
        private AnimationStateMapper<State> mapper;
        private int turnHash;
        private int hurtHash;
        private int deathHash;
        private bool turn;
        private bool doOnce;
        private bool walkingRight;
        private Vector3 left;
        private Vector3 right;

        protected override void LocalInitialize()
        {
            int layerIndex = this.anim.GetLayerIndex(this.animatorLayer);
            this.mapper = new AnimationStateMapper<State>(
                new List<string> {
                    this.animatorLayer + ".Idle",
                    this.animatorLayer + ".Turn",
                    this.animatorLayer + ".Hurt",
                    this.animatorLayer + ".Death"
                },
                this.anim,
                layerIndex);

            this.turnHash = Animator.StringToHash("Turn");
            this.hurtHash = Animator.StringToHash("Hurt");
            this.deathHash = Animator.StringToHash("Death");
        }

        protected override void LocalReInitialize()
        {
            this.currentState = State.Idle;
            this.walkingRight = true;
            this.doOnce = false;
            this.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        protected override void LocalUpdate()
        {
            if(!this.doOnce)
            {
                this.left = this.leftBoundary.position;
                this.right = this.rightBoundary.position;
                this.doOnce = true;
            }

            this.currentState = this.mapper.GetCurrentState();
            switch(this.currentState)
            {
                case State.Idle: Idle(); break;
                case State.Turn: Turn(); break;
                case State.Hurt: Idle(); break;
            }
        }

        protected override void LocalDeallocate()
        {
            this.rgbdy.velocity = Vector3.zero;
            this.anim.SetBool(this.deathHash, false);
        }

        protected override void LocalDelete()
        {
        }

        protected override void TakeDamage()
        {
            this.anim.SetTrigger(this.hurtHash);
        }

        protected override void Death()
        {
            this.anim.SetBool(this.deathHash, true);
            ReturnEnemy();
        }

        private void Idle()
        {
            if (this.turn)
            {
                this.turn = false;
                if (this.walkingRight)
                {
                    this.walkingRight = false;
                    this.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                }
                else
                {
                    this.walkingRight = true;
                    this.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                }
            }
            
            this.rgbdy.velocity = this.transform.right * this.speed;
            if ((this.transform.position.x > this.right.x && this.walkingRight) || (this.transform.position.x < this.left.x && !this.walkingRight))
                this.anim.SetTrigger(this.turnHash);
        }

        private void Turn()
        {
            this.turn = true;
            this.rgbdy.velocity = Vector3.zero;
        }
    }
}
