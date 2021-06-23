using System;
using Assets.Bullets.PlayerBullets;
using Assets.GameTasks;
using Assets.Hierarchy.ColorHandlers;
using Assets.UI;
using Assets.Util;
using LogUtilAssets;
using UnityEngine;

namespace Assets
{
    [FinalDebugViewLevelAttribute]
    public abstract class ValkyrieSprite : Loggable
    {
        #region Init

        protected virtual void OnInit() { }
        /// <summary>
        /// Init contains functionality to happen the first time
        /// this object is created.
        /// </summary>
        public void Init()
        {
            RetributionTimeScale = 1.0f;
            ColorHandler = DefaultColorHandler();
            OnInit();
        }
        public void Init(Vector3 position)
        {
            transform.position = position;
            Init();
        }

        public void Init(Vector3 position, Vector3 velocity)
        {
            Velocity = velocity;
            Init(position);
        }

        #endregion Init

        #region Frames

        public abstract TimeScaleType TimeScale { get; }

        public virtual bool IsPaused { get => false; }

        public float TotalTime { get; private set; }

        public virtual float TimeScaleModifier
        {
            get
            {
                float modifier = TimeScaleManager.GetTimeScaleModifier(TimeScale);
                return modifier;
            }
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime * RetributionTimeScale;
            float representedDt = deltaTime * TimeScaleModifier;

            RunFrame(representedDt, deltaTime);

            //if(this is Assets.Enemies.Enemy || this is SentinelBullet)
            //    Debug.DrawRay(transform.position, RepresentedVelocity, Color.red, float.Epsilon);
        }

        /// <summary>
        /// ValkyrieSprites will run frames based on their own modified time scale.
        /// They will also be provided with the true delta time since last frame.
        /// </summary>
        /// <param name="deltaTime">The amount of time since the last frame after applying the relevant time scale.</param>
        /// <param name="realDeltaTime">The true amount of time since the last frame before applying the relevant time scale.</param>
        protected virtual void OnFrameRun(float deltaTime, float realDeltaTime) { }
        /// <summary>
        /// ValkyrieSprites will run frames based on their own modified time scale.
        /// They will also be provided with the true delta time since last frame.
        /// </summary>
        /// <param name="deltaTime">The amount of time since the last frame after applying the relevant time scale.</param>
        /// <param name="realDeltaTime">The true amount of time since the last frame before applying the relevant time scale.</param>
        public void RunFrame(float deltaTime, float realDeltaTime)
        {
            TotalTime += deltaTime;

            ApplyVelocity(Velocity, deltaTime);

            OnFrameRun(deltaTime, realDeltaTime);

            //#if DEBUG
            //            if (TotalTime < 0.5f && RetributionTimeScale == 0.0f)
            //            {
            //                Log("RETRIBUTION TIME SCALE = 0.0f (Did you call Init?)");

            //                var renderer = GetComponent<Renderer>();
            //                var boundsOffset = renderer.bounds.size * 0.5f;
            //                var bottomLeft = transform.position - boundsOffset;

            //                var map = new BoxMap(bottomLeft, renderer.bounds.size);

            //                const float RedXTime = float.Epsilon;
            //                DebugUtil.RedX(map.TopLeft, RedXTime);
            //                DebugUtil.RedX(map.TopRight, RedXTime);
            //                DebugUtil.RedX(map.BottomLeft, RedXTime);
            //                DebugUtil.RedX(map.BottomRight, RedXTime);
            //            }
            //#endif
        }

        #region Retribution

        // Sprites affected by Retribution are slowed down.
        public float RetributionTimeScale { get; private set; }

        protected virtual void OnRetributionBulletCollisionEnter(RetributionBullet bullet) { }
        public void RetributionBulletCollisionEnter(RetributionBullet bullet)
        {
            SetRetributionTimeScale(bullet);
            OnRetributionBulletCollisionEnter(bullet);
        }

        public void SetRetributionTimeScale(RetributionBullet bullet)
        {
            RetributionTimeScale = bullet.RetributionTimeScaleValue;
        }

        public void ResetRetributionTimeScale()
        {
            RetributionTimeScale = 1.0f;
        }

        #endregion Retribution

        #endregion Frames

        #region Tasks

        public void RunTask(GameTask task)
        {
            GameManager.Instance.StartTask(task);
        }

        protected void ClearGameTasks()
        {
            GameManager.Instance.GameTaskRunnerDeactivated(this);
        }

        #endregion Tasks

        #region Color

        public ColorHandler ColorHandler { get; protected set; }
        protected abstract ColorHandler DefaultColorHandler();

        protected virtual void OnSpriteColorSet(Color color) { }
        public Color SpriteColor
        {
            get => ColorHandler.Color;
            set
            {
                ColorHandler.Color = value;
                OnSpriteColorSet(value);
            }
        }

        protected virtual void OnAlphaSet(float alpha) { }
        public float Alpha
        {
            get => ColorHandler.Alpha;
            set
            {
                ColorHandler.Alpha = value;
                OnAlphaSet(value);
            }
        }

        #endregion Color

        #region Sound

        public void PlaySoundAtCenter(AudioClip sound, float volumeScale = 1.0f)
        {
            float pan = SpaceUtil.PanFromPosition(PositionX);
            SoundManager.PlaySoundWithPan(sound, pan, volumeScale);
        }

        #endregion Sound

        #region Velocity

        /// <summary>
        /// The velocity that will be applied to this sprite each frame.
        /// </summary>
        public virtual Vector2 Velocity { get; set; }
        public float VelocityX
        {
            get => Velocity.x;
            set => Velocity = new Vector2(value, Velocity.y);
        }
        public virtual float VelocityY
        {
            get => Velocity.y;
            set => Velocity = new Vector2(Velocity.x, value);
        }

        public void ApplyVelocity(Vector2 velocity, float deltaTime)
        {
            Vector3 translate = new Vector3(velocity.x * deltaTime, velocity.y * deltaTime, 0);
            transform.Translate(translate, Space.World);
        }

        /// <summary>
        /// The absolute velocity of this sprite, theoretically equal to
        /// the Velocity property plus the velocities of any other forces
        /// currently acting to move the sprite.
        /// </summary>
        public virtual Vector2 RepresentedVelocity => Velocity;

        #endregion Velocity

        #region Transform Position

        public float PositionX
        {
            get => transform.position.x;
            set
            {
                transform.position = new Vector3(value, transform.position.y, transform.position.z);
            }
        }

        public float PositionY
        {
            get => transform.position.y;
            set
            {
                transform.position = new Vector3(transform.position.x, value, transform.position.z);
            }
        }

        public float PositionZ
        {
            get => transform.position.z;
            set
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, value);
            }
        }


        public float LocalPositionX
        {
            get => transform.localPosition.x;
            set
            {
                transform.localPosition = new Vector3(value, transform.localPosition.y, transform.localPosition.z);
            }
        }

        public float LocalPositionY
        {
            get => transform.localPosition.y;
            set
            {
                transform.localPosition = new Vector3(transform.localPosition.x, value, transform.localPosition.z);
            }
        }

        public float LocalPositionZ
        {
            get => transform.localPosition.z;
            set
            {
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, value);
            }
        }

        #endregion Transform Position

        #region Transform LocalScale

        public float LocalScale
        {
            get => transform.localScale.x;
            set
            {
                transform.localScale = new Vector3(value, value, transform.localScale.z);
            }
        }

        public float LocalScaleX
        {
            get => transform.localScale.x;
            set
            {
                transform.localScale = new Vector3(value, transform.localScale.y, transform.localScale.z);
            }
        }

        public float LocalScaleY
        {
            get => transform.localScale.y;
            set
            {
                transform.localScale = new Vector3(transform.localScale.x, value, transform.localScale.z);
            }
        }

        public float LocalScaleZ
        {
            get => transform.localScale.z;
            set
            {
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, value);
            }
        }

        #endregion Transform LocalScale

        #region Transform Rotation

        public float RotationDegrees
        {
            get => transform.eulerAngles.z;
            set
            {
                var rotation = transform.eulerAngles;
                rotation.z = value;
                transform.eulerAngles = rotation;
            }
        }

        public void RotateSprite(float rotation)
        {
            transform.Rotate(0, 0, rotation);
        }

        #endregion Transform Rotation

        #region Fleeting Text

        /// <summary>
        /// Calls GameManager.CreateFleetingText() using the given <paramref name="text"/>
        /// at the center of this object.
        /// </summary>
        /// <param name="text">The text to display.</param>
        public FleetingText CreateFleetingTextAtCenter(object text)
        {
            return CreateFleetingTextAtCenter(text.ToString());
        }

        /// <summary>
        /// Calls GameManager.CreateFleetingText() using the given <paramref name="text"/>
        /// at the center of this object.
        /// </summary>
        /// <param name="text">The text to display.</param>
        public FleetingText CreateFleetingTextAtCenter(string text)
        {
            FleetingText ret = GameManager.Instance.CreateFleetingText(text.ToString(), this.transform.position);
            return ret;
        }

        #endregion Fleeting Text
    }
}
