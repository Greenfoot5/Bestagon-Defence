using UnityEngine;

namespace Turrets.Upgrades.BulletUpgrades
{
    [CreateAssetMenu(fileName = "SlowUpgrade", menuName = "Upgrades/BulletUpgrade/SlowsEnemyUpgrade", order = 1)]
    public class SlowUpgrade : BulletUpgrade
    {
        [Range(0f, 1f)]
        [SerializeField]
        private float effectPercentage;
        
        public void AlterBulletSettings(ref Bullet bullet)
        {
            throw new System.NotImplementedException();
        }

        public override Bullet OnShoot(Bullet bullet)
        {
            bullet.AddUpgrade(this);
            return bullet;
        }

        public override void OnHit(Enemy target)
        {
            target.Slow(effectPercentage);
        }

        public new bool ValidUpgrade(ref Turret turret)
        {
            return turret.attackType == TurretType.Bullet;
        }
    }
}
