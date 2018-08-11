namespace PhaseShift.Character
{
    using System.Collections.Generic;
    using UnityEngine;
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

        private enum State { Idle, Aim, Shoot }

        private Camera sceneCamera;
        private AnimationOverrideHandler animOverride;
        private AnimationStateMapper<State> mapper;
        private State currentState;
        private Vector2 aimDir;
        private Vector2 mousePos;
        private int currentClipSet;
        private int aimHash;


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
            this.sceneCamera = FindObjectOfType<Camera>();
        }

        private void Update()
        {
            bool shouldAim = ShouldAim();
            if (shouldAim)
            {
                this.aimDir = GetAimDir();
                CalculateClipSet(Vector2.Angle(Vector2.right, this.aimDir));
            }

            this.anim.SetBool(this.aimHash, shouldAim);
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
            if(CustomInput.UsingPad)
            {
                float up = CustomInput.Bool(CustomInput.UserInput.LookUp) ? CustomInput.Raw(CustomInput.UserInput.LookUp) : CustomInput.Raw(CustomInput.UserInput.LookDown);
                float right = CustomInput.Bool(CustomInput.UserInput.LookRight) ? CustomInput.Raw(CustomInput.UserInput.LookRight) : CustomInput.Raw(CustomInput.UserInput.LookLeft);
                return up != 0 || right != 0;
            }
            else
                return !(mousePos != new Vector2(CustomInput.MouseX, CustomInput.MouseY));
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
                Vector3 norm = (new Vector3(CustomInput.MouseX, CustomInput.MouseY, 0) - this.sceneCamera.WorldToScreenPoint(this.transform.position)).normalized;
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

        }
    }
}
