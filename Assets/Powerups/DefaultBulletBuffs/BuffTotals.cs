using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Constants;
using Assets.Util;

namespace Assets.Powerups.DefaultBulletBuff
{
    public struct PosionDamageTotal
    {
        private SegmentedTotal SegmentedTotal;
        public int Total => SegmentedTotal.Total;
        public void Reset() => SegmentedTotal.Reset();

        public int VenomousRoundsDamage { get => SegmentedTotal.Item1; set => SegmentedTotal.Item1 = value; }

        [Obsolete(ObsoleteConstants.FollowTheFun)]
        public int SnakeBiteDamage { get => SegmentedTotal.Item2; set => SegmentedTotal.Item2 = value; }
    }

    public struct ParasiteDamageTotal
    {
        private SegmentedTotal SegmentedTotal;
        public int Total => SegmentedTotal.Total;
        public void Reset() => SegmentedTotal.Reset();

        public int InfestedRoundsDamage { get => SegmentedTotal.Item1; set => SegmentedTotal.Item1 = value; }
    }

    public struct AcidDamageTotal
    {
        private SegmentedTotal SegmentedTotal;
        public int Total => SegmentedTotal.Total;
        public void Reset() => SegmentedTotal.Reset();

        public int AcidicRoundsDamage { get => SegmentedTotal.Item1; set => SegmentedTotal.Item1 = value; }
    }

    public struct ChilledTimeTotal
    {
        private SegmentedTotal SegmentedTotal;
        public int Total => SegmentedTotal.Total;
        public void Reset() => SegmentedTotal.Reset();

        public int CryogenicRoundsTime { get => SegmentedTotal.Item1; set => SegmentedTotal.Item1 = value; }
    }

    public struct SilenceTimeTotal
    {
        private SegmentedTotal SegmentedTotal;
        public int Total => SegmentedTotal.Total;
        public void Reset() => SegmentedTotal.Reset();

        public int MutingRoundsTime { get => SegmentedTotal.Item1; set => SegmentedTotal.Item1 = value; }
    }


    public struct DamageScaleIncreaseTotal
    {
        private SegmentedTotalF SegmentedTotal;
        public float Total => SegmentedTotal.Total;
        public void Reset() => SegmentedTotal.Reset();

        public float AugmentedRoundsIncrease { get => SegmentedTotal.Item1; set => SegmentedTotal.Item1 = value; }
        public float CryogenicRoundsIncrease { get => SegmentedTotal.Item2; set => SegmentedTotal.Item2 = value; }
    }
}
