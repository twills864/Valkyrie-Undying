using Assets.Bullet;
using Assets.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : Enemy
{
    public override int BaseSpawnHealth => 100;
    public override float SpawnHealthScaleRate => 1.0f;

    protected override void _Init()
    {
        CurrentHealth = BaseSpawnHealth;
    }

    private void Start()
    {
        _Init();
    }

    protected override void _Update(float deltaTime)
    {

    }

    protected override void CollideWithBullet(Bullet bullet)
    {
        CurrentHealth -= bullet.Damage;
        UpdateHealthBar();
    }
}
