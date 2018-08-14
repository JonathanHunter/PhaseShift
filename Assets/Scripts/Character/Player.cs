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
        private GameObject phaser;
        [SerializeField]
        private Collider col;
        [SerializeField]
        private Animator anim;
        [SerializeField]
        private string animatorLayer;
        [SerializeField]
        private float speed;
        [SerializeField]
        private float jumpSpeed;
        [SerializeField]
        private Material background;

        [SerializeField]
        private AudioSource deathSound;
        [SerializeField]
        private AudioSource skateSound;

        public bool IsDead { get { return this.died; } }

        private enum State { Idle, Move, Jump, Fall, Death }
        private State currentState;
        private AnimationStateMapper<State> mapper;
        private int inAirHash;
        private int jumpHash;
        private int deathHash;
        private bool inTrigger;
        private bool wasPaused;
        private bool died;
        private Vector3 oldVel;

        private void Start()
        {
            this.currentState = State.Idle;
            int layerIndex = this.anim.GetLayerIndex(this.animatorLayer);
            this.mapper = new AnimationStateMapper<State>(
                new List<string> {
                    this.animatorLayer + ".Idle",
                    this.animatorLayer + ".Move",
                    this.animatorLayer + ".Jump",
                    this.animatorLayer + ".Fall",
                    this.animatorLayer + ".Death"
                }, 
                this.anim,
                layerIndex);
            this.inAirHash = Animator.StringToHash("InAir");
            this.jumpHash = Animator.StringToHash("Jump");
            this.deathHash = Animator.StringToHash("Death");
            this.inTrigger = false;
            this.died = false;
        }

        private void Update()
        {
            if (Managers.GameManager.Instance.IsPaused)
            {
                if (!this.wasPaused)
                {
                    this.oldVel = this.rgbdy.velocity;
                    this.wasPaused = true;
                }

                this.rgbdy.velocity = Vector3.zero;
                return;
            }

            if(this.wasPaused)
            {
                this.rgbdy.velocity = this.oldVel;
                this.wasPaused = false;
            }

            if (!this.inTrigger)
                this.phaser.layer = (int)Enums.Layers.Player;

            this.inTrigger = false;
            this.currentState = this.mapper.GetCurrentState();
            this.anim.SetBool(this.inAirHash, InAirCheck());
            switch (this.currentState)
            {
                case State.Idle: Idle(); break;
                case State.Move: Move(); break;
                case State.Jump: Jump(); break;
                case State.Fall: Fall(); break;
                case State.Death: Death(); break;
            }
            if (!InAirCheck() && IsMovingCheck())
            {
                skateSound.volume = 0.5f;
            }
            else
            {
                skateSound.volume = 0;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.tag == Enums.Tags.Enemy.ToString() || collision.gameObject.tag == Enums.Tags.EnemyBullet.ToString())
            {
                this.rgbdy.velocity = -this.transform.right * this.speed + this.transform.up * this.jumpSpeed;
                this.col.enabled = false;
                this.anim.SetTrigger(this.deathHash);
            }
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.tag == Enums.Tags.Space.ToString())
            {
                this.phaser.layer = (int)Enums.Layers.PlayerPhase;
                this.inTrigger = true;
            }
        }

        private void OnTriggerStay(Collider collision)
        {
            if (collision.gameObject.tag == Enums.Tags.Space.ToString())
            {
                this.phaser.layer = (int)Enums.Layers.PlayerPhase;
                this.inTrigger = true;
            }
        }

        private void OnTriggerExit(Collider collision)
        {
            if (collision.gameObject.tag == Enums.Tags.Space.ToString())
            {
                this.phaser.layer = (int)Enums.Layers.Player;
                this.inTrigger = true;
            }
        }

        private bool InAirCheck()
        {
            return this.rgbdy.velocity.y < -.5f || this.rgbdy.velocity.y > .5f;
        }

        private bool IsMovingCheck()
        {
            return this.rgbdy.velocity.x< -.1f || this.rgbdy.velocity.x > .1f;
        }

        private void Idle()
        {
            if(this.died)
            {
                this.anim.ResetTrigger(this.deathHash);
                this.col.enabled = true;
                this.died = false;
                Managers.CheckpointManager.Instance.Respawn();
            }
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
            this.anim.ResetTrigger(this.jumpHash);
            float xVel = GetXVel();
            this.rgbdy.velocity = new Vector3(xVel, this.rgbdy.velocity.y, 0);
        }

        private void Death()
        {
            if (!this.died)
            {
                deathSound.Play();
            }
            this.died = true;
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
