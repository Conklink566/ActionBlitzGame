using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Interface;
using Game.Level;

namespace Game.Gameplay
{
    /// <summary>
    /// Player states
    /// </summary>
    public enum PlayerStates
    {
        None            = 0,
        PlayerIdle      = 1,
        PlayerRun       = 2,
        PlayerDuck      = 3,
        PlayerFaint     = 4,
        PlayerJump      = 5
    };

    public class Player : MonoBehaviour
    {
        /// <summary>
        /// Running Collider
        /// </summary>
        public BoxCollider2D RunningCollider;

        /// <summary>
        /// Ducking Collider
        /// </summary>
        public BoxCollider2D DuckingCollider;

        /// <summary>
        /// Selected Collider
        /// </summary>
        public BoxCollider2D SelectedCollider { get; set; }

        /// <summary>
        /// Head Animator
        /// </summary>
        public Animator HeadIcon;

        /// <summary>
        /// Body Animator
        /// </summary>
        public Animator BodyIcon;

        /// <summary>
        /// Legs Animator
        /// </summary>
        public Animator LegsIcon;

        /// <summary>
        /// ArmBehind Animator
        /// </summary>
        public Animator ArmBehindIcon;

        /// <summary>
        /// Rigidbody2d component
        /// </summary>
        private Rigidbody2D _Rigidbody2D;

        /// <summary>
        /// Default Speed
        /// </summary>
        public float SpeedDefault;

        /// <summary>
        /// Jump power
        /// </summary>
        public float JumpPower;

        /// <summary>
        /// Amount of times the player can jump
        /// </summary>
        public float JumpAmount;

        /// <summary>
        /// Current Jump
        /// </summary>
        public float CurrentJump;

        /// <summary>
        /// If player is below killzone, the player faints
        /// </summary>
        public float KillZone;

        /// <summary>
        /// Time till maximum height of jump
        /// </summary>
        public float JumpTime = 1.0f;

        /// <summary>
        /// Current Jump Time
        /// </summary>
        public float CurrentJumpTime;

        /// <summary>
        /// Gravity
        /// </summary>
        public float Gravity;

        /// <summary>
        /// Causes the sliding to reduce speed by a lot
        /// </summary>
        public float SlideDrag = 1.5f;

        /// <summary>
        /// Falling check
        /// </summary>
        public bool Falling;

        /// <summary>
        /// To prevent falling and running state glitching
        /// </summary>
        public float FallCheckFineTuning;

        /// <summary>
        /// Fall check
        /// </summary>
        //private bool _FallCheck;

        /// <summary>
        /// Previous Position Y
        /// </summary>
        public float PreviousPositionY;

        /// <summary>
        /// Public variable for debuging
        /// </summary>
        public Vector2 fallCheck;

        /// <summary>
        /// Player State
        /// </summary>
        private AnimationType _PlayerState;

        /// <summary>
        /// Previous state before pausing
        /// </summary>
        public AnimationType PreviousState { get; set; }

        /// <summary>
        /// Player State
        /// </summary>
        public AnimationType PlayerState
        {
            get
            {
                return this._PlayerState;
            }
            set
            {
                this._PlayerState = value;

                this.HeadIcon.SetBool("Idle", AnimationType.Idle == this._PlayerState);
                this.HeadIcon.SetBool("Duck", AnimationType.Duck == this._PlayerState);
                this.HeadIcon.SetBool("Run", AnimationType.Run == this._PlayerState);
                this.HeadIcon.SetBool("Jump", AnimationType.Jump == this._PlayerState);
                this.HeadIcon.SetBool("Faint", AnimationType.Faint == this._PlayerState);

                this.BodyIcon.SetBool("Idle", AnimationType.Idle == this._PlayerState);
                this.BodyIcon.SetBool("Duck", AnimationType.Duck == this._PlayerState);
                this.BodyIcon.SetBool("Run", AnimationType.Run == this._PlayerState);
                this.BodyIcon.SetBool("Jump", AnimationType.Jump == this._PlayerState);
                this.BodyIcon.SetBool("Faint", AnimationType.Faint == this._PlayerState);

                this.LegsIcon.SetBool("Idle", AnimationType.Idle == this._PlayerState);
                this.LegsIcon.SetBool("Duck", AnimationType.Duck == this._PlayerState);
                this.LegsIcon.SetBool("Run", AnimationType.Run == this._PlayerState);
                this.LegsIcon.SetBool("Jump", AnimationType.Jump == this._PlayerState);
                this.LegsIcon.SetBool("Faint", AnimationType.Faint == this._PlayerState);

                this.ArmBehindIcon.SetBool("Idle", AnimationType.Idle == this._PlayerState);
                this.ArmBehindIcon.SetBool("Duck", AnimationType.Duck == this._PlayerState);
                this.ArmBehindIcon.SetBool("Run", AnimationType.Run == this._PlayerState);
                this.ArmBehindIcon.SetBool("Jump", AnimationType.Jump == this._PlayerState);
                this.ArmBehindIcon.SetBool("Faint", AnimationType.Faint == this._PlayerState);

                this.RunningCollider.enabled = AnimationType.Duck != this._PlayerState;
                this.SelectedCollider = AnimationType.Duck != this._PlayerState ? this.RunningCollider : this.DuckingCollider;
                this.DuckingCollider.enabled = AnimationType.Duck == this._PlayerState;

            }
        }

        /// <summary>
        /// Movement
        /// </summary>
        public Vector2 _Movement;

        /// <summary>
        /// Awake this instance
        /// </summary>
        private void Awake()
        {
            this._Rigidbody2D = this.GetComponent<Rigidbody2D>();
            this.PlayerState = AnimationType.Idle;
            this.ChangeAnimatorLayerSet(CharacterBodyType.Head, FileConfigHandler.Instance.UserConfig.CharacterConfig.HeadType);
            this.ChangeAnimatorLayerSet(CharacterBodyType.Body, FileConfigHandler.Instance.UserConfig.CharacterConfig.BodyType);
            this.ChangeAnimatorLayerSet(CharacterBodyType.Legs, FileConfigHandler.Instance.UserConfig.CharacterConfig.LegsType);
        }

        /// <summary>
        /// Start this isntance
        /// </summary>
        private void Start()
        {
            this.PreviousPositionY = this.transform.position.y;
        }

        /// <summary>
        /// Update every frame
        /// </summary>
        private void FixedUpdate()
        {
            if (Manager.Instance.GameState == GameState.Pause ||
                !Manager.Instance.StartGame)
            {
                Manager.Instance.BackgroundMovement.Adjuster = 0.0f;
                return;
            }
            float speed = (this.SpeedDefault + (this.SpeedDefault * (Manager.Instance.SpeedCurrentModifier / 100.0f) * Manager.Instance.SpeedMagnifier)) * Time.fixedDeltaTime;
            if (Manager.Instance.GameState == GameState.Lose)
                speed = 0.0f;
            float moveForward = this.transform.position.x + speed;
            float upSpeed = this.JumpCalculation((this.CurrentJumpTime == 0 ? -Time.fixedDeltaTime : (this.CurrentJumpTime - Time.fixedDeltaTime)), this.JumpPower / (this.CurrentJump <= 0 ? 1 : this.CurrentJump));
            float upWard = this.transform.position.y + upSpeed;
            this._Movement = new Vector2(moveForward, upWard);
            this._Rigidbody2D.MovePosition(this._Movement);
            Manager.Instance.BackgroundMovement.Adjuster = this._Movement.x;
            if (this.FallCheck(speed, upSpeed))
            {
                //print("falling");
                if (this.PlayerState != AnimationType.Jump && Manager.Instance.GameState != GameState.Lose && this.PlayerState != AnimationType.Duck)
                    this.PlayerState = AnimationType.Jump;
                if (!this.Falling)
                {
                    this.Falling = true;
                    this.CurrentJump++;
                }
                this.CurrentJumpTime -= Time.fixedDeltaTime;
            }
            else
            {
                if (this.Falling)
                {
                    this.Falling = false;
                    this.CurrentJump = 0;
                    this.CurrentJumpTime = 0.0f;
                }
                //print("running");
                if (this.PlayerState != AnimationType.Run && Manager.Instance.GameState != GameState.Lose && this.PlayerState != AnimationType.Duck)
                    this.PlayerState = AnimationType.Run;
            }
            if(this.transform.position.y < this.KillZone)
            {
                Manager.Instance.GameState = GameState.Lose;
                this.CurrentJumpTime = 0.0f;
            }
        }

        /// <summary>
        /// On Collision Enter
        /// </summary>
        /// <param name="collision"></param>
        public void OnCollisionEnter2D(Collision2D collision)
        {
            Obstacle obstacle = collision.gameObject.GetComponent<Obstacle>();
            if (obstacle == null)
                return;
            bool gotHit = false;
            foreach(ContactPoint2D contactPoint2D in collision.contacts)
            {
                Player player = collision.otherCollider.gameObject.GetComponent<Player>();
                if (player == null)
                    continue;
                float sizeY = collision.gameObject.GetComponent<BoxCollider2D>().size.y;
                float posY = collision.gameObject.transform.position.y;
                if(contactPoint2D.point.y < posY + (sizeY / 2.0f))
                {
                    gotHit = true;
                    break;
                }
            }
            if (gotHit)
            {
                Manager.Instance.GameState = GameState.Lose;
                this.CurrentJumpTime = 0.0f;
            }
        }

        /// <summary>
        /// Player Jump
        /// </summary>
        public void Jump()
        {
            if (this.CurrentJump >= this.JumpAmount)
                return;
            this.PlayerState = AnimationType.Jump;
            this.CurrentJumpTime = this.JumpTime;
        }

        /// <summary>
        /// Player duck
        /// </summary>
        public void Duck()
        {
            this.PlayerState = AnimationType.Duck;
        }

        /// <summary>
        /// Jump Calculation
        /// </summary>
        /// <param name="time"></param>
        /// <param name="power"></param>
        /// <returns></returns>
        private float JumpCalculation(float time, float power)
        {
            float calculation = 0.0f;
            calculation = (time > 0 ? 1.0f : -1.0f) * power * (Mathf.Pow(time, 2.0f));
            //print(string.Format("time: {0}, power: {1}, Math.power: {2}, Calculation: {3}", time, power, Mathf.Pow(time, 2.0f), calculation));
            float normalized = (time > 0 ? 1.0f : -1.0f) * power * (Mathf.Pow(this.JumpTime, 2.0f));
            if (Mathf.Abs(calculation) > Mathf.Abs(normalized))
            {
                calculation = normalized;
            }
            return calculation;
        }

        /// <summary>
        /// Fall Check
        /// </summary>
        /// <returns></returns>
        private bool FallCheck(float speedX, float jumpSpeedY)
        {
            if (this.CurrentJumpTime > 0.0f)
            {
                return true;
            }
            fallCheck = new Vector2(speedX, jumpSpeedY);
            Vector2 point_L = new Vector2(this.transform.position.x - this.SelectedCollider.offset.x - (this.SelectedCollider.size.x / 2.0f),
                                          this.transform.position.y + this.SelectedCollider.offset.y - (this.SelectedCollider.size.y / 2.0f) - this.FallCheckFineTuning);
            Vector2 point_R = new Vector2(this.transform.position.x + this.SelectedCollider.offset.x + (this.SelectedCollider.size.x / 2.0f),
                                          this.transform.position.y + this.SelectedCollider.offset.y - (this.SelectedCollider.size.y / 2.0f) - this.FallCheckFineTuning);
            RaycastHit2D[] hitList_L = Physics2D.RaycastAll(point_L, fallCheck, Vector2.Distance(point_L, new Vector2(point_L.x + fallCheck.x, point_L.y + fallCheck.y)));
            RaycastHit2D[] hitList_R = Physics2D.RaycastAll(point_R, fallCheck, Vector2.Distance(point_R, new Vector2(point_R.x + fallCheck.x, point_R.y + fallCheck.y)));
            Debug.DrawRay(point_L, fallCheck, Color.green);
            Debug.DrawRay(point_R, fallCheck, Color.green);
            for (int i = 0; i < hitList_L.Length; i++)
            {
                if (hitList_L[i].collider.gameObject == this.gameObject)
                    continue;
                return false;
            }
            for(int i = 0; i < hitList_R.Length; i++)
            {
                if (hitList_R[i].collider.gameObject == this.gameObject)
                    continue;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Change Animator Layer Set
        /// </summary>
        public void ChangeAnimatorLayerSet(CharacterBodyType type, int layerType)
        {
            switch (type)
            {
                case CharacterBodyType.Head:
                    for (int i = 0; i < this.HeadIcon.layerCount; i++)
                    {
                        this.HeadIcon.SetLayerWeight(i, i == layerType ? 1.0f : 0.0f);
                    }
                    break;
                case CharacterBodyType.Body:
                    for (int i = 0; i < this.BodyIcon.layerCount; i++)
                    {
                        this.BodyIcon.SetLayerWeight(i, i == layerType ? 1.0f : 0.0f);
                    }
                    for (int i = 0; i < this.ArmBehindIcon.layerCount; i++)
                    {
                        this.ArmBehindIcon.SetLayerWeight(i, i == layerType ? 1.0f : 0.0f);
                    }
                    break;
                case CharacterBodyType.Legs:
                    for (int i = 0; i < this.LegsIcon.layerCount; i++)
                    {
                        this.LegsIcon.SetLayerWeight(i, i == layerType ? 1.0f : 0.0f);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}