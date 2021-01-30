using Assets;
using Assets.Bullet;
using Assets.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : PooledObject
{
    public int CurrentHealth { get; set; }

    // The health this enemy spawns with at the start of the game
    public abstract int BaseSpawnHealth { get; }

    // The rate at which the spawn health of this enemy increases as the game progresses
    public abstract float SpawnHealthScaleRate { get; }

    [SerializeField]
    public EnemyHealthBar HealthBar;

    public void UpdateHealthBar() => HealthBar.SetText(CurrentHealth);

    public override string LogTagColor => "#FFB697";

    public override void Init()
    {
        HealthBar = FindChildEnemyHealthBar();
        HealthBar.Init();
        _Init();

        UpdateHealthBar();
    }
    protected abstract void _Init();

    private void Start()
    {
        Init();
    }

    protected abstract void CollideWithBullet(Bullet bullet);
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CollisionUtil.IsPlayerBullet(collision))
        {
            Bullet bullet = collision.GetComponent<Bullet>();
            CollideWithBullet(bullet);
        }
    }

    protected abstract void _Update(float deltaTime);
    private void Update()
    {
        _Update(Time.deltaTime);
    }

    private EnemyHealthBar FindChildEnemyHealthBar()
    {
        var healthBarTransform = gameObject.transform.Find("EnemyHealthBar");
        var gObject = healthBarTransform.gameObject;
        var ret = gObject.GetComponent<EnemyHealthBar>();
        return ret;
    }
}
