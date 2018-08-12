namespace PhaseShift.Character
{
    using System.Collections.Generic;
    using UnityEngine;
    using Bullets;
    using Util;

    public class Gun : MonoBehaviour
    {
        [SerializeField]
        private Animator anim;
        [SerializeField]
        private string animatorLayer;
        [SerializeField]
        private AnimationClip[] aim;
        [SerializeField]
        private AnimationClip[] shoot;
        [SerializeField]
        private Transform barrel;
        [SerializeField]
        private float shotTime;
        [SerializeField]
        private float spaceTime;

        private enum State { Idle, Aim, Shoot }

        private Camera sceneCamera;
        private AnimationOverrideHandler animOverride;
        private AnimationStateMapper<State> mapper;
        private State currentState;
        private Vector2 aimDir;
        private Vector3 mousePos;
        private int currentClipSet;
        private int aimHash;
        private int shootHash;
        private float shotTimer;


        private void Start()
        {
            this.currentState = State.Idle;
            int layerIndex = this.anim.GetLayerIndex(this.animatorLayer);
            this.mapper = new AnimationStateMapper<State>(
                new List<string> {
                    this.animatorLayer + ".Idle",
                    this.animatorLayer + ".Aim",
                    this.animatorLayer + ".Shoot"
                },
                this.anim,
                layerIndex);

            this.animOverride = new AnimationOverrideHandler(this.anim);
            this.aimDir = Vector2.zero;
            this.mousePos = Vector2.zero;
            this.currentClipSet = 0;
            this.aimHash = Animator.StringToHash("Aim");
            this.shootHash = Animator.StringToHash("Shoot");
            this.sceneCamera = FindObjectOfType<Camera>();
        }

        private void Update()
        {
            if (Managers.GameManager.Instance.IsPaused)
                return;

            bool shouldAim = ShouldAim();
            if (shouldAim)
            {
                this.aimDir = GetAimDir();
                //CalculateClipSet(Vector2.SignedAngle(Vector2.right, this.aimDir));
            }

            bool shouldShoot = false;
            if (this.shotTimer > 0)
                this.shotTimer -= Time.deltaTime;
            else if (CustomInput.BoolHeld(CustomInput.UserInput.Shoot1) || CustomInput.BoolHeld(CustomInput.UserInput.Shoot2))
                shouldShoot = true;

            this.anim.SetBool(this.aimHash, shouldAim);
            this.anim.SetBool(this.shootHash, shouldShoot);
            this.currentState = this.mapper.GetCurrentState();
            switch(this.currentState)
            {
                case State.Idle: Idle(); break;
                case State.Aim: Aim(); break;
                case State.Shoot: Shoot(); break;
            }
        }

        private bool ShouldAim()
        {
            if (CustomInput.UsingPad)
            {
                float up = CustomInput.Bool(CustomInput.UserInput.LookUp) ? CustomInput.Raw(CustomInput.UserInput.LookUp) : CustomInput.Raw(CustomInput.UserInput.LookDown);
                float right = CustomInput.Bool(CustomInput.UserInput.LookRight) ? CustomInput.Raw(CustomInput.UserInput.LookRight) : CustomInput.Raw(CustomInput.UserInput.LookLeft);
                return up != 0 || right != 0;
            }
            else
            {
                if (this.mousePos != new Vector3(CustomInput.MouseX, CustomInput.MouseY, 0))
                {
                    this.mousePos = new Vector3(CustomInput.MouseX, CustomInput.MouseY, 0);
                    return true;
                }

                return false;
            }
        }

        private Vector2 GetAimDir()
        {
            float up, right;
            if (CustomInput.UsingPad)
            {
                up = CustomInput.Bool(CustomInput.UserInput.LookUp) ? CustomInput.Raw(CustomInput.UserInput.LookUp) : CustomInput.Raw(CustomInput.UserInput.LookDown);
                right = CustomInput.Bool(CustomInput.UserInput.LookRight) ? CustomInput.Raw(CustomInput.UserInput.LookRight) : CustomInput.Raw(CustomInput.UserInput.LookLeft);
            }
            else
            {
                Vector3 norm = (this.mousePos - this.sceneCamera.WorldToScreenPoint(this.transform.position)).normalized;
                up = norm.y;
                right = norm.x;
            }

            return new Vector2(right, up);
        }

        private void CalculateClipSet(float angle)
        {
            if (angle < 0)
                angle += 360f;

            int clip = 0;
            if (angle < 45)
                clip = 0;
            else if (angle < 90)
                clip = 1;
            else if (angle < 135)
                clip = 2;
            else if (angle < 180)
                clip = 3;
            else if (angle < 225)
                clip = 4;
            else if (angle < 270)
                clip = 5;
            else if (angle < 315)
                clip = 6;
            else
                clip = 7;

            if (clip != this.currentClipSet)
            {
                this.animOverride.OverrideClip(this.aim[0], this.aim[clip]);
                this.animOverride.OverrideClip(this.shoot[0], this.shoot[clip]);
                this.animOverride.ApplyOverrides();
                this.currentClipSet = clip;
            }
        }

        private void Idle()
        {
        }

        private void Aim()
        {
        }

        private void Shoot()
        {
            if (shotTimer <= 0)
            {
                if (CustomInput.BoolHeld(CustomInput.UserInput.Shoot2))
                {
                    GameObject b = BulletPool.Instance.GetBullet(BulletPool.BulletTypes.Basic);
                    if (b != null)
                    {
                        b.transform.position = this.barrel.position;
                        b.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, this.aimDir));
                        this.shotTimer = this.shotTime;
                    }
                }
                else if (CustomInput.BoolHeld(CustomInput.UserInput.Shoot1))
                {
                    GameObject b = BulletPool.Instance.GetBullet(BulletPool.BulletTypes.Basic);
                    if (b != null)
                    {
                        b.transform.position = this.barrel.position;
                        b.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, this.aimDir));
                        this.shotTimer = this.spaceTime;
                    }
                }
            }
        }
    }
}
