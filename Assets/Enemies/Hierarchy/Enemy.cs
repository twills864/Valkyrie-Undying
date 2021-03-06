using System.Diagnostics;
using Assets.Bullets.PlayerBullets;
using Assets.FireStrategies.EnemyFireStrategies;
using Assets.GameTasks;
using Assets.Hierarchy.ColorHandlers;
using Assets.ObjectPooling;
using Assets.Powerups;
using Assets.UI;
using Assets.Util;
using UnityEngine;

namespace Assets.Enemies
{
    /// <inheritdoc/>
    public abstract class Enemy : PooledObject, IVictimHost
    {
        public override string LogTagColor => "#FFB697";
        public override GameTaskType TaskType => GameTaskType.Enemy;

        [SerializeField]
        protected SpriteRenderer Sprite;

        protected override ColorHandler DefaultColorHandler()
            => new SpriteColorHandler(Sprite);

        protected virtual bool ShouldDeactivateOnDestructor => true;
        public virtual bool CanVoidPause => true;

        /// <summary>
        /// Enemies below this Y limit will not be allowed to fire their weapons.
        /// </summary>
        public static float FireHeightFloor { get; set; }

        public int PointValue { get; set; }
        public int CurrentHealth { get; set; }

        // The health this enemy spawns with at the start of the game
        public abstract int BaseSpawnHealth { get; }

        // The rate at which the spawn health of this enemy increases as the game progresses
        public abstract float SpawnHealthScaleRate { get; }

        public bool IsVictim
        {
            get => GameManager.Instance.VictimEnemy == this;
            set
            {
                if (value)
                    GameManager.Instance.VictimEnemy = this;
                else if (IsVictim)
                    GameManager.Instance.VictimEnemy = null;
            }
        }

        [SerializeField]
        private float _victimMarkerDistance;
        public virtual float VictimMarkerDistance => _victimMarkerDistance;
        private VictimMarker _victimMarker;
        public VictimMarker VictimMarker
        {
            get => _victimMarker;
            set
            {
                _victimMarker = value;
                value.Host = this;
            }
        }


        public virtual Vector2 FirePosition => SpriteMap.Bottom;
        protected virtual bool CanFire(Vector2 firePosition) => firePosition.y > FireHeightFloor;


        [SerializeField]
        private Vector3 HealthBarOffsetScale;

        protected virtual Vector3 HealthBarOffset => InitialHealthbarOffset;
        protected Vector3 InitialSize { get; private set; }
        protected Vector3 InitialHealthbarOffset { get; private set; }

        public EnemyHealthBar HealthBar { get; set; }
        public void UpdateHealthBar() => HealthBar.SetText(CurrentHealth);

        public SpriteBoxMap SpriteMap { get; protected set; }
        public ColliderBoxMap ColliderMap { get; protected set; }


        public abstract EnemyFireStrategy FireStrategy { get; protected set; }
        public LoopingFrameTimer FireTimer => FireStrategy.FireTimer;

        #region Inferno

        protected virtual float InfernoTickTime => 0.75f;
        private LoopingFrameTimer InfernoTimer { get; set; }

        protected virtual float InfernoDamageScale => 1f;
        public virtual int InfernoDamageIncrease { get; set; }
        private int InfernoDamage { get; set; }
        public bool IsBurning { get; set; }

        #endregion Inferno

        #region Void

        private int _voidPauseCounter;
        protected int VoidPauseCounter
        {
            get => _voidPauseCounter;
            set
            {
                _voidPauseCounter = value;
                IsPaused = IsVoidPaused;
            }
        }
        protected bool IsVoidPaused => VoidPauseCounter > 0;

        private Vector3 VoidPausedVelocity { get; set; }

        public void VoidPause()
        {
            if(CanVoidPause)
            {
                if (!IsVoidPaused)
                {
                    VoidPausedVelocity = Velocity;
                    Velocity = Vector2.zero;
                }
                VoidPauseCounter++;
            }
        }
        public void VoidResume()
        {
            if (CanVoidPause)
            {
                VoidPauseCounter--;

                if (!IsVoidPaused)
                    Velocity = VoidPausedVelocity;
            }
        }

        #endregion Void

        protected virtual void OnEnemyInit() { }
        protected sealed override void OnInit()
        {
            SpriteMap = new SpriteBoxMap(this);
            ColliderMap = new ColliderBoxMap(this);

            var collider = gameObject.GetComponent<Collider2D>();
            var boundsSize = collider.bounds.size;
            InitialSize = new Vector3(boundsSize.x, boundsSize.y, 0);

            float healthbarOffsetYScale = HealthBarOffsetScale.y >= 0 ? 0.5f : -0.5f;
            var healthbarOffset = new Vector3(0, EnemyHealthBar.HealthBarHeight * healthbarOffsetYScale, 0);
            InitialHealthbarOffset = Vector3.Scale(InitialSize, HealthBarOffsetScale)
                + healthbarOffset
                ;


            InfernoTimer = new LoopingFrameTimer(InfernoTickTime);
            OnEnemyInit();
        }

        protected virtual void OnEnemyActivate() { }
        protected sealed override void OnActivate()
        {
            FireStrategy.Reset();
            InfernoTimer.Reset();

            CurrentHealth = BaseSpawnHealth;
            PointValue = CurrentHealth;
            IsBurning = false;
            VoidPauseCounter = 0;

            OnEnemyActivate();
        }

        protected virtual void OnEnemySpawn() { }
        public sealed override void OnSpawn()
        {
            var healthBarSpawn = transform.position;// + HealthBarOffset;
            HealthBar = PoolManager.Instance.UIElementPool.Get<EnemyHealthBar>(healthBarSpawn);
            UpdateHealthBar();

            OnEnemySpawn();
        }

        protected virtual void OnEnemyFrame(float deltaTime) { }
        protected sealed override void OnManagedVelocityObjectFrameRun(float deltaTime)
        {
            if (IsBurning && InfernoTimer.UpdateActivates(deltaTime))
            {
                if (BurnKills())
                    return;
            }

            if (!IsVoidPaused)
            {
                if (FireTimer.UpdateActivates(deltaTime))
                    FireBullets();

                OnEnemyFrame(deltaTime);
            }

            HealthBar.transform.position = (Vector3)ColliderMap.Center + HealthBarOffset;
        }

        protected virtual void FireBullets()
        {
            if (CanFire(FirePosition))
            {
                var bullets = FireStrategy.GetBullets(FirePosition);
            }
        }

        protected virtual bool BurnKills()
        {
            bool ret;
            if (!DamageKills(InfernoDamage))
            {
                InfernoDamage += InfernoDamageIncrease;
                ret = false;
            }
            else
            {
                KillEnemy(null);
                ret = true;
            }
            return ret;
        }

        private void Start()
        {
            //BoxMap = new TrackedBoxMap(this);
            //FireStrategy = DefaultEnemyFireStrategy;
            //HealthBar = FindChildEnemyHealthBar();
            //Init();
        }

        public virtual bool DamageKills(int damage)
        {
            if (!isActiveAndEnabled)
                return false;

            CurrentHealth -= damage;
            if (CurrentHealth <= 0)
                return true;

            UpdateHealthBar();
            return false;
        }
        protected virtual void CollideWithBullet(PlayerBullet bullet)
        {
            if (DamageKills(bullet.Damage))
                KillEnemy(bullet);

            bullet.OnCollideWithEnemy(this);
            GameManager.Instance.OnEnemyHit(this, bullet);
        }


        protected virtual void OnDeath() { }
        protected void KillEnemy(PlayerBullet bullet)
        {
            // Kill enemy
            CreateFleetingTextAtCenter(PointValue);
            GameManager.Instance.OnEnemyKill(this, bullet);
            DeactivateSelf();
            OnDeath();
        }

        protected virtual void OnEnemyDeactivate() { }
        protected sealed override void OnDeactivate()
        {
            if (IsVictim)
                IsVictim = false;

            HealthBar.DeactivateSelf();
            HealthBar = null;

            OnEnemyDeactivate();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (CollisionUtil.IsPlayerBullet(collision))
            {
                PlayerBullet bullet = collision.GetComponent<PlayerBullet>();
                if (bullet.CollidesWithEnemy(this))
                    CollideWithBullet(bullet);
            }
            else if (CollisionUtil.IsPlayer(collision))
            {
                if (Player.Instance.CollideWithEnemy(this))
                    DeactivateSelf();
            }
            if (CollisionUtil.IsEnemy(collision) && IsBurning)
            {
                Enemy enemy = collision.GetComponent<Enemy>();
                enemy.Ignite(InfernoPowerup.CurrentBaseDamage, InfernoPowerup.CurrentDamageIncreasePerTick);
            }
        }

        protected void OnTriggerExit2D(Collider2D collision)
        {
            if (CollisionUtil.IsDestructor(collision))
            {
                if (ShouldDeactivateOnDestructor)
                    DeactivateSelf();
            }
        }

        public virtual Vector2 RandomShrapnelPosition()
        {
            var topLeft = SpriteMap.TopLeft;
            var maxX = SpriteMap.Right.x;
            var x = RandomUtil.Float(topLeft.x, maxX);

            var ret = new Vector2(x, topLeft.y);
            return ret;
        }

        public virtual Vector2 ShrapnelPosition(PlayerBullet bullet)
        {
            var collisionX = bullet.transform.position.x;
            return ShrapnelPosition(collisionX);
        }
        public virtual Vector2 ShrapnelPosition(float collisionX)
        {
            var y = ColliderMap.Top.y;

            var ret = new Vector2(collisionX, y);
            return ret;
        }

        public void Ignite(int baseDamage, int damageIncreasePerTick)
        {
            int newInfernoDamageIncrease = (int)(InfernoDamageScale * damageIncreasePerTick);
            int newInfernoDamage = (int)(InfernoDamageScale * baseDamage) + newInfernoDamageIncrease;

            if (!IsBurning)
            {
                InfernoDamage = newInfernoDamage;
                IsBurning = true;
                HealthBar.Ignite();
                InfernoDamageIncrease = newInfernoDamageIncrease;
            }
            else
            {
                if (InfernoDamageIncrease < newInfernoDamageIncrease)
                    InfernoDamageIncrease = newInfernoDamageIncrease;
                if (InfernoDamage < newInfernoDamage)
                    InfernoDamage = newInfernoDamage;
            }
        }

        private EnemyHealthBar FindChildEnemyHealthBar()
        {
            var healthBarTransform = gameObject.transform.Find("EnemyHealthBar");
            var gObject = healthBarTransform.gameObject;
            var ret = gObject.GetComponent<EnemyHealthBar>();
            return ret;
        }

        //private void Update()
        //{
        //    const float redXTime = float.Epsilon;
        //    DebugUtil.RedX(BoxMap.TopLeft, redXTime);
        //    DebugUtil.RedX(BoxMap.TopRight, redXTime);
        //    DebugUtil.RedX(BoxMap.BottomLeft, redXTime);
        //    DebugUtil.RedX(BoxMap.BottomRight, redXTime);
        //}



        //private void OnMouseEnter()
        //{
        //    GameManager.Instance.VictimEnemy = this;

        //    DebugUtil.RedX(transform.position);
        //    GameManager.Instance.CreateFleetingText("ENTER", SpaceUtil.WorldMap.Center);
        //}

        //// Having trouble making this work correctly
        //private void OnMouseDown()
        //{
        //    //GameManager.Instance.VictimEnemy = this;
        //    //DebugUtil.RedX(transform.position);
        //    GameManager.Instance.CreateFleetingText("ENEMY ONMOUSEDOWN", SpaceUtil.WorldMap.Center);
        //}

        public void DebugKill()
        {
            CurrentHealth = 0;
            KillEnemy(null);
        }
    }
}