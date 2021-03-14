using Assets.GameTasks;
using Assets.Hierarchy.ColorHandlers;
using Assets.UI;
using LogUtilAssets;
using UnityEngine;

namespace Assets
{
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

        #region Spawn

        /// <summary>
        /// Subclass-specific functionality to happen
        /// immediately after an object is activated.
        /// </summary>
        protected virtual void OnValkyrieSpriteActivate() { }

        /// <summary>
        /// Subclass-specific functionality to happen after an object is spawned
        /// and has relevant and useful information assigned to it.
        /// </summary>
        protected virtual void OnValkyrieSpriteSpawn() { }

        #endregion Spawn

        #region Frames

        public abstract GameTaskType TaskType { get; }

        public virtual bool IsPaused { get => false; }

        public float TotalTime { get; private set; }

        protected virtual void OnFrameRun(float deltaTime) { }
        /// <summary>
        /// ValkyrieSprites will manually handle frame updates
        /// instead of relying on Unity's implementation of Update().
        /// </summary>
        /// <param name="deltaTime">The amount of time since the last frame.</param>
        public void RunFrame(float deltaTime)
        {
            TotalTime += deltaTime;

            ApplyVelocity(Velocity, deltaTime);

            OnFrameRun(deltaTime);
        }

        /// <summary>
        /// Runs the frame only if this object is enabled.
        /// </summary>
        /// <param name="deltaTime">The amount of time since the last frame.</param>
        public void RunFrameIfEnabled(float deltaTime)
        {
            if (gameObject.activeSelf)
                RunFrame(deltaTime);
        }


        #endregion Frames

        #region Tasks

        public void RunTask(GameTask task)
        {
            GameManager.Instance.StartTask(task, TaskType);
        }

        protected void ClearGameTasks()
        {
            GameManager.Instance.GameTaskRunnerDeactivated(this);
        }

        #endregion Tasks

        #region Color

        protected ColorHandler ColorHandler { get; private set; }
        protected abstract ColorHandler DefaultColorHandler();

        public Color SpriteColor
        {
            get => ColorHandler.Color;
            set => ColorHandler.Color = value;
        }

        public float Alpha
        {
            get => ColorHandler.Alpha;
            set => ColorHandler.Alpha = value;
        }

        #endregion Color

        #region Velocity

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

        #endregion Transform Position

        #region Transform LocalScale

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
