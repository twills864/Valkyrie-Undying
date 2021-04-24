using System;
using System.Diagnostics;
using Assets.Bullets.PlayerBullets;
using Assets.Constants;
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
        public override TimeScaleType TimeScale => TimeScaleType.Enemy;

        #region Prefabs

        // The health this enemy spawns with at the start of the game.
        [SerializeField]
        private float _InitialSpawnHealth = GameConstants.PrefabNumber;

        // The rate at which the spawn health of this enemy increases as the game progresses.
        [SerializeField]
        private float _HealthScaling = GameConstants.PrefabNumber;

        // Powerup drop chance multiplier will default to the HealthScaling value.
        // It can be overridden here.
        [SerializeField]
        private EnemyPowerupDropChanceOverride _PowerupDropChanceOverride = default;

        [SerializeField]
        private Tuple<bool, int> _lol;

        [SerializeField]
        private SpriteRenderer _Sprite = null;

        [SerializeField]
        private float _VictimMarkerDistance = GameConstants.PrefabNumber;

        [SerializeField, Obsolete(MetronomePowerup.MetronomeObsolete)]
        private MetronomeLabel _metronomeLabel = null;

        [SerializeField]
        private Vector3 _HealthBarOffsetScale = Vector3.zero;

        [SerializeField, Obsolete("Use _FirstSpawnMinute")]
        private int _DifficultyLevel = GameConstants.PrefabNumber;

        [SerializeField]
        private float _FirstSpawnMinute = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        // The health this enemy spawns with at the start of the game.
        protected float InitialSpawnHealth => _InitialSpawnHealth;

        // The rate at which the spawn health of this enemy increases as the game progresses.
        protected float HealthScaling => _HealthScaling;

        private float? PowerupDropChanceMultiplierOverride => _PowerupDropChanceOverride.Value;

        protected SpriteRenderer Sprite => _Sprite;

        public virtual float VictimMarkerDistance => _VictimMarkerDistance;

        private Vector3 HealthBarOffsetScale => _HealthBarOffsetScale;

        /// <summary>
        /// Enemies will be introduced to the game in order of their difficulty level.
        /// </summary>
        [SerializeField, Obsolete("Use FirstSpawnMinute")]
        public int DifficultyLevel => _DifficultyLevel;

        /// <summary>
        /// The earliest minute of total game time that this enemy can spawn.
        /// </summary>
        public float FirstSpawnMinute => _FirstSpawnMinute;

        #endregion Prefab Properties


        protected override ColorHandler DefaultColorHandler()
            => new SpriteColorHandler(Sprite);

        protected virtual bool ShouldDeactivateOnDestructor => true;

        public int PointValue { get; set; }

        public SpriteBoxMap SpriteMap { get; protected set; }
        public ColliderBoxMap ColliderMap { get; protected set; }


        #region Valkyrie Sprite Methods

        protected virtual void OnEnemyInit() { }
        protected sealed override void OnInit()
        {
            SpriteMap = new SpriteBoxMap(this);
            ColliderMap = new ColliderBoxMap(this);

            var collider = gameObject.GetComponent<Collider2D>();
            var boundsSize = collider.bounds.size;
            var initialSize = new Vector3(boundsSize.x, boundsSize.y, 0);

            float healthbarOffsetYScale = HealthBarOffsetScale.y >= 0 ? 0.5f : -0.5f;
            var healthbarOffset = new Vector3(0, EnemyHealthBar.HealthBarHeight * healthbarOffsetYScale, 0);
            InitialHealthbarOffset = Vector3.Scale(initialSize, HealthBarOffsetScale)
                + healthbarOffset;

            InfernoTimer = new LoopingFrameTimer(InfernoTickTime);

            FireStrategy = InitialFireStrategy();

            OnEnemyInit();
        }

        protected virtual void OnEnemyActivate() { }
        protected sealed override void OnActivate()
        {
            InfernoTimer.Reset();

            IsBurning = false;
            VoidPauseCounter = 0;

            OnEnemyActivate();
        }

        protected virtual void OnEnemySpawn() { }
        public sealed override void OnSpawn()
        {
            float increase = HealthScaling * Director.EnemyHealthIncrease;
            CurrentHealth = (int)(InitialSpawnHealth + increase);
            PointValue = CurrentHealth;

            var healthBarSpawn = transform.position;// + HealthBarOffset;
            HealthBar = PoolManager.Instance.UIElementPool.Get<EnemyHealthBar>(healthBarSpawn);
            UpdateHealthBar();

            WasKilled = false;

#if UNITY_EDITOR

            ActiveInDirector = false;
            DespawnHandledByDirector = false;

#endif

            Director.EnemySpawned(this);

            OnEnemySpawn();
        }

        protected virtual void OnEnemyFrame(float deltaTime, float realDeltaTime) { }
        protected sealed override void OnFrameRun(float deltaTime, float realDeltaTime)
        {
            if (IsBurning && InfernoTimer.UpdateActivates(deltaTime))
            {
                if (BurnKills())
                    return;
            }

            if (!IsVoidPaused)
            {
                OnEnemyFrame(deltaTime, realDeltaTime);
            }

            HealthBar.transform.position = (Vector3)ColliderMap.Center + HealthBarOffset;

#if UNITY_EDITOR
            DebugUpdate(deltaTime, realDeltaTime);
#endif
        }

        protected virtual void OnEnemyDeactivate() { }
        protected sealed override void OnDeactivate()
        {
            Director.EnemyDeactivated(this);

            if (IsVictim)
                IsVictim = false;

            HealthBar.DeactivateSelf();
            HealthBar = null;

#if UNITY_EDITOR
            if(!DespawnHandledByDirector)
            {
                const string Message = "ERROR: ENEMY DEACTIVATED THAT WAS NOT HANDLED BY DIRECTOR.";
                Log(Message);
                GameManager.Instance.CreateFleetingText(Message, transform.position);
                GameManager.Instance.CreateFleetingText(Message, SpaceUtil.WorldMap.Center);
            }
#endif

            OnEnemyDeactivate();
        }

        #endregion Valkyrie Sprite Methods

        #region Firing

        /// <summary>
        /// Enemies below this Y limit will not be allowed to fire their weapons.
        /// </summary>
        public static float FireHeightFloor { get; set; }

        public EnemyFireStrategy FireStrategy { get; protected set; }
        protected abstract EnemyFireStrategy InitialFireStrategy();

        public virtual Vector3 FirePosition => SpriteMap.Bottom;
        protected virtual bool CanFire(Vector3 firePosition) => firePosition.y > FireHeightFloor;

        #endregion Firing

        #region Health

        public int CurrentHealth { get; set; }

        protected virtual Vector3 HealthBarOffset => InitialHealthbarOffset;
        protected Vector3 InitialHealthbarOffset { get; private set; }

        public EnemyHealthBar HealthBar { get; set; }
        public void UpdateHealthBar() => HealthBar.SetText(CurrentHealth);

        #endregion Health

        #region Damage

        public bool WasKilled { get; set; }
        public virtual bool InfluencesDirectorGameBalance => true;

        public float PowerupDropChanceMultiplier => PowerupDropChanceMultiplierOverride ?? HealthScaling;

        protected virtual bool DamageKills(int damage)
        {
            if (!isActiveAndEnabled)
                return false;

            CurrentHealth -= damage;
            if (CurrentHealth <= 0)
                return true;

            UpdateHealthBar();
            return false;
        }

        public virtual void TrueDamage(int damage, PlayerBullet bullet = null)
        {
            if (isActiveAndEnabled && DamageKills(damage))
                KillEnemy(bullet);
        }

        protected virtual void OnDeath() { }
        public void KillEnemy(PlayerBullet bullet)
        {
            WasKilled = true;
            GameManager.Instance.OnEnemyKill(this, bullet);
            DeactivateSelf();
            OnDeath();
        }

        #endregion Damage

        #region Powerup Effects

        #region Smite

        public virtual bool DiesOnSmite => true;

        #endregion Smite

        #region Victim

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

        #endregion Victim

        #region Metronome

        [Obsolete(MetronomePowerup.MetronomeObsolete)]
        public bool HasMetronome
        {
            get => GameManager.Instance.MetronomeEnemy == this;
            set
            {
                if (value)
                    GameManager.Instance.MetronomeEnemy = this;
                else if (IsVictim)
                    GameManager.Instance.MetronomeEnemy = null;
            }
        }

        [Obsolete(MetronomePowerup.MetronomeObsolete)]
        public MetronomeLabel MetronomeLabel
        {
            get => _metronomeLabel;
            set
            {
                _metronomeLabel = value;
                value.Host = this;
            }
        }

        #endregion Metronome

        #region Inferno

        protected virtual float InfernoTickTime => 1.0f;
        private LoopingFrameTimer InfernoTimer { get; set; }

        protected virtual float InfernoDamageScale => 1f;
        public virtual int InfernoDamageIncrease { get; protected set; }
        protected int InfernoDamage { get; set; }
        public bool IsBurning { get; set; }

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

        #endregion Inferno

        #region Void

        public virtual bool CanVoidPause => true;

        private int _voidPauseCounter;
        protected int VoidPauseCounter;

        public override bool IsPaused => IsVoidPaused;
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

        #endregion Powerup Effects

        #region Collision

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
                if (Player.Instance.CollidesWithEnemy(this))
                    KillEnemy(null);
            }
            if (CollisionUtil.IsEnemy(collision) && IsBurning)
            {
                Enemy enemy = collision.GetComponent<Enemy>();
                enemy.Ignite(InfernoPowerup.CurrentBaseDamage, InfernoPowerup.CurrentDamageIncreasePerTick);
            }
        }

        public virtual void CollideWithBullet(PlayerBullet bullet)
        {
            if (gameObject.activeSelf)
            {
                if (DamageKills(bullet.Damage))
                    KillEnemy(bullet);

                GameManager.Instance.OnEnemyHit(this, bullet);
                bullet.OnCollideWithEnemy(this);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (gameObject.activeSelf)
            {
                if (CollisionUtil.IsDestructor(collision))
                {
                    if (ShouldDeactivateOnDestructor)
                    {
                        DeactivateSelf();
                    }
                }
            }
        }

        #endregion Collision

        #region Debug

#if UNITY_EDITOR
        private void DebugUpdate(float deltaTime, float realDeltaTime)
        {
            const float RedXTime = float.Epsilon;

            if (!ActiveInDirector)
            {
                Log("ERROR: ACTIVE ENEMY NOT ACCOUNTED FOR BY DIRECTOR");
                DebugUtil.RedX(ColliderMap.TopLeft, RedXTime);
                DebugUtil.RedX(ColliderMap.TopRight, RedXTime);
                DebugUtil.RedX(ColliderMap.BottomLeft, RedXTime);
                DebugUtil.RedX(ColliderMap.BottomRight, RedXTime);
            }
        }
#endif

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

#if UNITY_EDITOR

        public bool ActiveInDirector { get; set; }
        public bool DespawnHandledByDirector { get; set; }

#endif

        public void DebugKill()
        {
            CurrentHealth = 0;
            KillEnemy(null);
        }

        #endregion Debug
    }
}