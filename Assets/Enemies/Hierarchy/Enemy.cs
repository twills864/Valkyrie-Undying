using Assets.Bullets.PlayerBullets;
using Assets.FireStrategies.EnemyFireStrategies;
using Assets.GameTasks;
using Assets.Util;
using UnityEngine;

namespace Assets.Enemies
{
    /// <inheritdoc/>
    public abstract class Enemy : PooledObject
    {
        public override string LogTagColor => "#FFB697";
        public override GameTaskType TaskType => GameTaskType.Enemy;

        public int PointValue { get; set; }
        public int CurrentHealth { get; set; }

        // The health this enemy spawns with at the start of the game
        public abstract int BaseSpawnHealth { get; }

        // The rate at which the spawn health of this enemy increases as the game progresses
        public abstract float SpawnHealthScaleRate { get; }

        [SerializeField]
        public EnemyHealthBar HealthBar;

        public virtual Vector2 FirePosition => BoxMap.Bottom;

        public void UpdateHealthBar() => HealthBar.SetText(CurrentHealth);

        protected TrackedBoxMap BoxMap { get; set; }

        public abstract EnemyFireStrategy FireStrategy { get; protected set; }
        public LoopingFrameTimer FireTimer => FireStrategy.FireTimer;

        protected virtual void OnEnemyInit() { }
        public sealed override void OnInit()
        {
            BoxMap = new TrackedBoxMap(this);
            HealthBar = FindChildEnemyHealthBar();
            OnActivate();
            OnEnemyInit();
        }

        protected virtual void OnEnemyActivate() { }
        protected sealed override void OnActivate()
        {
            FireStrategy.Reset();
            CurrentHealth = BaseSpawnHealth;
            PointValue = CurrentHealth;
            UpdateHealthBar();

            OnEnemyActivate();
        }

        protected virtual void OnEnemyFrame(float deltaTime) { }
        protected sealed override void OnManagedVelocityObjectFrameRun(float deltaTime)
        {
            if(FireTimer.UpdateActivates(deltaTime))
                FireBullets();

            OnEnemyFrame(deltaTime);
        }

        protected virtual void FireBullets()
        {
            var bullets = FireStrategy.GetBullets(FirePosition);
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
            CurrentHealth -= damage;
            if (CurrentHealth <= 0)
                return true;

            UpdateHealthBar();
            return false;
        }
        protected virtual void CollideWithBullet(PlayerBullet bullet)
        {
            if (DamageKills(bullet.Damage))
                KillEnemy();

            bullet.OnCollideWithEnemy(this);
            GameManager.Instance.OnEnemyHit(this, bullet);
        }


        protected virtual void OnDeath() { }
        protected void KillEnemy()
        {
            // Kill enemy
            DeactivateSelf();
            CreateFleetingTextAtCenter(PointValue);
            OnDeath();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (CollisionUtil.IsPlayerBullet(collision))
            {
                PlayerBullet bullet = collision.GetComponent<PlayerBullet>();
                if(bullet.CollidesWithEnemy(this))
                    CollideWithBullet(bullet);
            }
            else if (CollisionUtil.IsPlayer(collision))
            {
                if (Player.Instance.CollideWithEnemy(this))
                    DeactivateSelf();
            }
        }

        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            if (CollisionUtil.IsDestructor(collision))
            {
                CollideWithDestructor();
            }
        }
        protected virtual void CollideWithDestructor()
        {
            DeactivateSelf();
        }

        public virtual Vector2 RandomShrapnelPosition()
        {
            var topLeft = BoxMap.TopLeft;
            var maxX = BoxMap.Right.x;
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
            var y = BoxMap.Top.y;

            var ret = new Vector2(collisionX, y);
            return ret;
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
    }
}