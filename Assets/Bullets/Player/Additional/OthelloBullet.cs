using Assets.Bullets.PlayerBullets;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public class OthelloBullet : PermanentVelocityPlayerBullet
    {
        public override int Damage => OthelloDamage;
        public int OthelloDamage { get; set; }
    }
}