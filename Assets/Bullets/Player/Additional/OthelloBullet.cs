using Assets.Bullets.PlayerBullets;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public class OthelloBullet : PermanentVelocityPlayerBullet
    {
        public override int Damage => base.Damage + DamageIncrease;
        public int DamageIncrease { get; set; }
    }
}