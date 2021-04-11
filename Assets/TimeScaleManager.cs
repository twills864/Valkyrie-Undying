using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Util;

namespace Assets
{
    public static class TimeScaleManager
    {
        public static float Player = 1.0f;
        public static float PlayerBullet = 1.0f;
        public static float Enemy = 1.0f;
        public static float EnemyBullet = 1.0f;
        public static float UIElement = 1.0f;

        public static float GetTimeScaleModifier(TimeScaleType type)
        {
            float ret;
            switch (type)
            {
                case TimeScaleType.Player: ret = Player; break;
                case TimeScaleType.PlayerBullet: ret = PlayerBullet; break;
                case TimeScaleType.Enemy: ret = Enemy; break;
                case TimeScaleType.EnemyBullet: ret = EnemyBullet; break;
                case TimeScaleType.UIElement: ret = UIElement; break;
                case TimeScaleType.Default: ret = 1f; break;
                default: throw ExceptionUtil.ArgumentException(() => type);
            }
            return ret;
        }
    }
}
