using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Util;
using UnityEngine;

namespace Assets
{
    public static class SoundBank
    {
        private const string PathSoundEffect = "Audio/Sounds/";

        /// <summary>
        /// NULL
        /// </summary>
        [Obsolete("TODO")]
        public static AudioClip Silence { get; private set; }

        /// <summary>
        /// flameThrower
        /// </summary>
        public static AudioClip Flare { get; private set; }

        /// <summary>
        /// 3shot
        /// </summary>
        public static AudioClip LaserBasic { get; private set; }

        /// <summary>
        /// laser3
        /// </summary>
        [Obsolete]
        public static AudioClip LaserGeneric { get; private set; }

        /// <summary>
        /// laser5
        /// </summary>
        public static AudioClip LaserPuff { get; private set; }

        /// <summary>
        /// laser6
        /// </summary>
        public static AudioClip LaserPew { get; private set; }

        /// <summary>
        /// laserCharge
        /// </summary>
        public static AudioClip LaserCharge { get; private set; }

        /// <summary>
        /// rocket
        /// </summary>
        public static AudioClip ZapHard { get; private set; }

        /// <summary>
        /// sfx_exp_long4
        /// </summary>
        public static AudioClip ExplosionLongDeep { get; private set; }

        /// <summary>
        /// sfx_exp_long5
        /// </summary>
        public static AudioClip ExplosionLongDramatic { get; private set; }

        /// <summary>
        /// sfx_exp_medium6
        /// </summary>
        public static AudioClip ExplosionMediumDeep { get; private set; }

        /// <summary>
        /// sfx_exp_medium10
        /// </summary>
        public static AudioClip ExplosionMediumZap { get; private set; }

        /// <summary>
        /// sfx_exp_short_hard6
        /// </summary>
        public static AudioClip ExplosionShortDeep { get; private set; }

        /// <summary>
        /// sfx_exp_short_soft7
        /// </summary>
        public static AudioClip ExplosionShortSweep { get; private set; }

        /// <summary>
        /// sfx_exp_shortest_hard1
        /// </summary>
        public static AudioClip ExplosionShortestHard { get; private set; }

        /// <summary>
        /// sfx_exp_shortest_hard3
        /// </summary>
        public static AudioClip ExplosionShortestIgnite { get; private set; }

        /// <summary>
        /// sfx_exp_shortest_hard5
        /// </summary>
        public static AudioClip ExplosionShortestCannonFire { get; private set; }

        /// <summary>
        /// sfx_exp_shortest_soft8
        /// </summary>
        public static AudioClip ExplosionShortestDeepTacticalFire { get; private set; }

        /// <summary>
        /// sfx_exp_shortest_soft9
        /// </summary>
        public static AudioClip ExplosionShortestKnock { get; private set; }

        /// <summary>
        /// sfx_sounds_impact7
        /// </summary>
        public static AudioClip ImpactShort { get; private set; }

        /// <summary>
        /// sfx_weapon_shotgun1
        /// </summary>
        public static AudioClip ShotgunHard { get; private set; }

        /// <summary>
        /// sfx_weapon_shotgun2
        /// </summary>
        public static AudioClip ShotgunWide { get; private set; }

        /// <summary>
        /// sfx_weapon_shotgun3
        /// </summary>
        public static AudioClip ShotgunStrong { get; private set; }

        /// <summary>
        /// sfx_weapon_singleshot3
        /// </summary>
        public static AudioClip GunPistol { get; private set; }

        /// <summary>
        /// sfx_weapon_singleshot5
        /// </summary>
        public static AudioClip GunShort { get; private set; }

        /// <summary>
        /// sfx_weapon_singleshot8
        /// </summary>
        public static AudioClip GunPew { get; private set; }

        /// <summary>
        /// sfx_weapon_singleshot19
        /// </summary>
        public static AudioClip GunShortStrong { get; private set; }

        /// <summary>
        /// sfx_wpn_cannon2
        /// </summary>
        public static AudioClip CannonStrong { get; private set; }

        /// <summary>
        /// sfx_wpn_laser5
        /// </summary>
        public static AudioClip LaserSwish { get; private set; }

        /// <summary>
        /// sfx_wpn_laser7
        /// </summary>
        public static AudioClip LaserGritty { get; private set; }

        /// <summary>
        /// sfx_wpn_laser8
        /// </summary>
        public static AudioClip LaserBrief { get; private set; }

        /// <summary>
        /// superLaser
        /// </summary>
        public static AudioClip LaserDramatic { get; private set; }

        /// <summary>
        /// weird_03
        /// </summary>
        public static AudioClip WaterDrop { get; private set; }

        /// <summary>
        /// weird_03
        /// </summary>
        public static AudioClip WaterDrip { get; private set; }

        /// <summary>
        /// cannon2
        /// </summary>
        [Obsolete("TODO")]
        public static AudioClip Cannon2 { get; private set; }

        /// <summary>
        /// forceField_000
        /// </summary>
        public static AudioClip ForceField0 { get; private set; }

        /// <summary>
        /// forceField_001
        /// </summary>
        public static AudioClip ForceField1 { get; private set; }

        /// <summary>
        /// forceField_002
        /// </summary>
        public static AudioClip ForceField2 { get; private set; }

        /// <summary>
        /// forceField_003
        /// </summary>
        public static AudioClip ForceField3 { get; private set; }

        /// <summary>
        /// forceField_004
        /// </summary>
        public static AudioClip ForceField4 { get; private set; }

        public static void Init()
        {
            Flare = LoadSoundEffect("flameThrower");
            LaserBasic = LoadSoundEffect("3shot");
            LaserGeneric = LoadSoundEffect("laser3");
            LaserPuff = LoadSoundEffect("laser5");
            LaserPew = LoadSoundEffect("laser6");
            LaserCharge = LoadSoundEffect("laserCharge");
            ZapHard = LoadSoundEffect("rocket");
            ExplosionLongDeep = LoadSoundEffect("sfx_exp_long4");
            ExplosionLongDramatic = LoadSoundEffect("sfx_exp_long5");
            ExplosionMediumDeep = LoadSoundEffect("sfx_exp_medium6");
            ExplosionMediumZap = LoadSoundEffect("sfx_exp_medium10");
            ExplosionShortDeep = LoadSoundEffect("sfx_exp_short_hard6");
            ExplosionShortSweep = LoadSoundEffect("sfx_exp_short_soft7");
            ExplosionShortestHard = LoadSoundEffect("sfx_exp_shortest_hard1");
            ExplosionShortestIgnite = LoadSoundEffect("sfx_exp_shortest_hard3");
            ExplosionShortestCannonFire = LoadSoundEffect("sfx_exp_shortest_hard5");
            ExplosionShortestDeepTacticalFire = LoadSoundEffect("sfx_exp_shortest_soft8");
            ExplosionShortestKnock = LoadSoundEffect("sfx_exp_shortest_soft9");
            ImpactShort = LoadSoundEffect("sfx_sounds_impact7");
            ShotgunHard = LoadSoundEffect("sfx_weapon_shotgun1");
            ShotgunWide = LoadSoundEffect("sfx_weapon_shotgun2");
            ShotgunStrong = LoadSoundEffect("sfx_weapon_shotgun3");
            GunPistol = LoadSoundEffect("sfx_weapon_singleshot3");
            GunShort = LoadSoundEffect("sfx_weapon_singleshot5");
            GunPew = LoadSoundEffect("sfx_weapon_singleshot8");
            GunShortStrong = LoadSoundEffect("sfx_weapon_singleshot19");
            CannonStrong = LoadSoundEffect("sfx_wpn_cannon2");
            LaserSwish = LoadSoundEffect("sfx_wpn_laser5");
            LaserGritty = LoadSoundEffect("sfx_wpn_laser7");
            LaserBrief = LoadSoundEffect("sfx_wpn_laser8");
            LaserDramatic = LoadSoundEffect("superLaser");
            WaterDrop = LoadSoundEffect("weird_03");
            WaterDrip = LoadSoundEffect("weird_04");

            ForceField0 = LoadSoundEffect("forceField_000");
            ForceField1 = LoadSoundEffect("forceField_001");
            ForceField2 = LoadSoundEffect("forceField_002");
            ForceField3 = LoadSoundEffect("forceField_003");
            ForceField4 = LoadSoundEffect("forceField_004");

            ForceFields = new AudioClip[]
            {
                ForceField0,
                ForceField1,
                ForceField2,
                ForceField3,
                ForceField4
            };
        }

        private static AudioClip LoadSoundEffect(string name)
        {
            string path = $"{PathSoundEffect}{name}";

            AudioClip ret = Resources.Load<AudioClip>(path);
            return ret;
        }

        private static AudioClip[] ForceFields { get; set; }
        public static AudioClip RandomForceField => RandomUtil.RandomElement(ForceFields);
    }
}
