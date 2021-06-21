using System;
using System.Collections.Generic;
using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Enemies;
using Assets.FireStrategies.PlayerFireStrategies;
using Assets.GameTasks;
using Assets.ObjectPooling;
using Assets.Particles;
using Assets.Pickups;
using Assets.Powerups;
using Assets.UI;
using LogUtilAssets;
using UnityEngine;

namespace Assets.Util
{
    public static class DebugUtil
    {
        public static string DebugEnemyName = "DebugEnemy";

        public static Color ColorRed => new Color(1.0f, 0.5f, 0.5f);

        public static DebugUI DebugUi { get; set; }
        private static GameManager GameManager { get; set; }

        public static void Init(DebugUI debugUi, GameManager gameManager)
        {
            DebugUi = debugUi;
            GameManager = gameManager;
        }

        public static Color GetRandomPlayerColor()
        {
            const float fixedColor = 0.5f;
            var fixedColorSelector = RandomUtil.Int(3);

            Color ret;
            switch (fixedColorSelector)
            {
                case 0:
                    ret = new Color(fixedColor, RandomUtil.Float(), RandomUtil.Float());
                    break;
                case 1:
                    ret = new Color(RandomUtil.Float(), fixedColor, RandomUtil.Float());
                    break;
                case 2:
                    ret = new Color(RandomUtil.Float(), RandomUtil.Float(), fixedColor);
                    break;
                default:
                    throw new Exception($"Unknown color selector {nameof(fixedColorSelector)}");
            }

            return ret;
        }


        #region Input Methods

        private static void InputSpace(KeyCode keyCode)
        {
            var position = SpaceUtil.WorldPositionUnderMouse();
            float scale = 10.0f;
            float duration = DebugUi.DebugTextBox.GetFloat(2.0f);
            int level = DebugUi.DebugTextBox.GetInt(1);
            VoidBullet.StartVoid(position, level, scale, duration);
            //RetributionBullet.StartRetribution(position, level, scale, duration);
        }

        private static void InputEnter(KeyCode keyCode)
        {
        }

        private static void InputPageUp(KeyCode keyCode)
        {
            DebugUi.AddGameSpeed(0.5f);
        }

        private static void InputPageDown(KeyCode keyCode)
        {
            DebugUi.AddGameSpeed(-0.5f);
        }

        private static void InputW(KeyCode keyCode)
        {
            GameManager._DebugEnemy.DebugDeathEffect();
            //Vector3 position = SpaceUtil.WorldPositionUnderMouse();
            //Vector3 velocity = new Vector3(0, -1f);
            //Color32 color = new Color32(byte.MaxValue, 0, 0, byte.MaxValue);
            //int count = 2;
            //ParticleManager.Instance.Emit(position, velocity, color, count);
        }

        private static void InputA(KeyCode keyCode)
        {

        }

        private static void InputS(KeyCode keyCode)
        {
            ParticleManager.Instance.EmitTest();
        }

        private static void InputD(KeyCode keyCode)
        {
            PoolManager.Instance.BulletPool.DebugPrintTrails();
        }

        private static void InputP(KeyCode keyCode)
        {
            PowerupSuite();
        }

        private static void InputE(KeyCode keyCode)
        {
            SpawnSpecificEnemy<NomadEnemy>();
            //SpawnSpecificEnemy<WaspEnemy>();
            //SpawnAllEnemies();
        }

        private static void InputC(KeyCode keyCode)
        {
            RainCurrentPowerup();
        }

        private static void InputUp(KeyCode keyCode)
        {
            GameManager.DebugDecrementFireType();
        }

        private static void InputLeft(KeyCode keyCode)
        {
            if (SpaceUtil.TryGetEnemyUnderMouse(out Enemy enemy))
            {
                enemy.AddParasites(3);
                //enemy.Ignite(1, 1, 3);
            }
        }

        private static void InputDown(KeyCode keyCode)
        {
            GameManager.DebugIncrementFireType();
        }

        private static void InputRight(KeyCode keyCode)
        {
            if (SpaceUtil.TryGetEnemyUnderMouse(out Enemy enemy))
            {
                enemy.AddAcid(4);
            }
        }

        private static void InputMouseBack(KeyCode keyCode)
        {
            if(SpaceUtil.TryGetEnemyUnderMouse(out Enemy enemy))
            {
                //enemy.Ignite(1, 1, 3);
                enemy.AddChill(1);
            }

            //SpawnSpecificEnemy<BasicEnemy>();
            //SpawnSpecificEnemy<CradleEnemy>();
            //SpawnSpecificEnemy<RingEnemy>();
            //SpawnSpecificEnemy<TankEnemy>();
            //SpawnSpecificEnemy<LaserEnemy>();
        }

        private static void InputMouseForward(KeyCode keyCode)
        {
            if (SpaceUtil.TryGetGameObjectUnderMouse(out GameObject gameObject))
            {
                if (gameObject.name != "Destructor")
                {
                    LogUtil.Log(gameObject);

                    if(gameObject.TryGetComponent<Enemy>(out Enemy enemy) && enemy.name != DebugEnemyName)
                        enemy.DebugKill();
                }
            }

            SpawnSpecificEnemy<LaserEnemy>();

            //SmiteBullet.DebugTestSmite();

            //RainCurrentPowerup();
            //PickupSpam();

            //TestRetribution();
        }

        private static void InputKeypadPlus(KeyCode keyCode)
        {

        }

        private static void InputKeypadMinus(KeyCode keyCode)
        {

        }

        private static void InputEscape(KeyCode key)
        {
            DebugUi.ToggleUI();

#if UNITY_EDITOR
            var debugEnemy = GameManager.Instance._DebugEnemy;
            if (debugEnemy.isActiveAndEnabled)
                debugEnemy.DeactivateSelf();
#endif

        }

        #endregion Input Methods


        #region Input Engine

        private class KeyCodeAction
        {
            public KeyCode KeyCode { get; set; }
            public Action<KeyCode> Action { get; set; }

            public KeyCodeAction(KeyCode keyCode, Action<KeyCode> action)
            {
                KeyCode = keyCode;
                Action = action;
            }

            public void Run()
            {
                Action(KeyCode);
            }
        }

        private static readonly List<KeyCodeAction> KeyCodeActions = new List<KeyCodeAction>()
        {
            new KeyCodeAction(KeyCode.W, InputW),
            new KeyCodeAction(KeyCode.A, InputA),
            new KeyCodeAction(KeyCode.S, InputS),
            new KeyCodeAction(KeyCode.D, InputD),
            new KeyCodeAction(KeyCode.P, InputP),
            new KeyCodeAction(KeyCode.E, InputE),
            new KeyCodeAction(KeyCode.C, InputC),
            new KeyCodeAction(KeyCode.UpArrow, InputUp),
            new KeyCodeAction(KeyCode.LeftArrow, InputLeft),
            new KeyCodeAction(KeyCode.DownArrow, InputDown),
            new KeyCodeAction(KeyCode.RightArrow, InputRight),
            new KeyCodeAction(KeyCode.PageUp, InputPageUp),
            new KeyCodeAction(KeyCode.PageDown, InputPageDown),
            new KeyCodeAction(KeyCode.Return, InputEnter),
            new KeyCodeAction(KeyCode.Space, InputSpace),
            new KeyCodeAction(KeyCode.Mouse3, InputMouseBack),
            new KeyCodeAction(KeyCode.Mouse4, InputMouseForward),
            new KeyCodeAction(KeyCode.KeypadPlus, InputKeypadPlus),
            new KeyCodeAction(KeyCode.KeypadMinus, InputKeypadMinus),
            new KeyCodeAction(KeyCode.Escape, InputEscape),
        };

        public static void HandleInput()
        {
            if (Input.anyKeyDown)
            {
                foreach (var pair in KeyCodeActions)
                {
                    if (Input.GetKeyDown(pair.KeyCode))
                    {
                        pair.Run();
                    }
                }
            }

            if (Input.GetMouseButton(2))
                GameManager._DebugEnemy.transform.position = SpaceUtil.WorldPositionUnderMouse();
        }

        #endregion Input Engine


        #region Visual Feedback

        #region Red X

        /// <summary>
        /// Draws a red X for a brief time at the specified position.
        /// </summary>
        /// <param name="position">The position to draw the X.</param>
        public static void RedX(Vector3 position, float rayTime = 1.5f)
        {
            const float XRadius = 0.3f;

            Vector3 topLeft = position + new Vector3(-XRadius, XRadius);
            Vector3 topRight = position + new Vector3(XRadius, XRadius);
            Vector3 bottomRight = position + new Vector3(XRadius, -XRadius);
            Vector3 bottomLeft = position + new Vector3(-XRadius, -XRadius);

            Debug.DrawLine(topLeft, bottomRight, ColorRed, rayTime);
            Debug.DrawLine(bottomLeft, topRight, ColorRed, rayTime);
        }

        /// <summary>
        /// Draws a red X for a brief time at the specified position, and
        /// displays a red Fleeting Text at this position.
        /// </summary>
        /// <param name="position">The position to draw the X.</param>
        /// <param name="message">The message to display.</param>
        public static void RedX(Vector3 position, string message)
        {
            RedX(position);

            const float YOffset = 0.5f;

            position.y += YOffset;
            var fleetingText = GameManager.Instance.CreateFleetingText(message, position);
            var text = fleetingText.GetComponent<TextMesh>();
            text.color = ColorRed;
        }

        /// <summary>
        /// Draws a red X for a brief time at the specified position, and
        /// displays a red Fleeting Text at this position.
        /// </summary>
        /// <param name="position">The position to draw the X.</param>
        /// <param name="message">The message to display.</param>
        public static void RedX(Vector3 position, object message)
        {
            RedX(position, message.ToString());
        }

        #endregion Red X

        #endregion Visual Feedback


        #region Timing

        /// <summary>
        /// Times a given <paramref name="action"/> using the Stopwatch class,
        /// and prints the result to the Debug log.
        /// </summary>
        /// <param name="name">The name of the action.</param>
        /// <param name="action">The action to time.</param>
        /// <returns>The amount of time the action took.</returns>
        public static TimeSpan TimeAction(Action action, string name = "Action")
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            action();
            sw.Stop();

            TimeSpan elapsed = sw.Elapsed;

            Debug.Log($"[{name}] {elapsed}");

            return elapsed;
        }

        #endregion Timing


        #region Get Types

        public static Type GetPowerupType<TPowerup>() where TPowerup : Powerup => typeof(TPowerup);
        public static Type GetOverrideFireStrategyType<TFireStrategy>() where TFireStrategy : PlayerFireStrategy => typeof(TFireStrategy);
        public static Type GetOverrideEnemyType<TEnemy>() where TEnemy : Enemy => typeof(TEnemy);

        #endregion Get Types


        #region Test Methods

        private static void PickupSpam()
        {
            var pool = PoolManager.Instance.PickupPool;

            const int NumPickups = 1;

            Vector3 mousePos = SpaceUtil.WorldPositionUnderMouse();
            const float PosDelta = 0.1f;

            for (int i = 0; i < NumPickups; i++)
            {
                float x = RandomUtil.Float(-PosDelta, PosDelta);
                float y = RandomUtil.Float(-PosDelta, PosDelta);
                Vector3 spawnPos = new Vector3(x, y) + mousePos;

                pool.GetRandomBasicPowerupPickup(spawnPos).OnSpawn();
            }
        }

        private static void RainCurrentPowerup()
        {
            Vector3 mousePos = SpaceUtil.WorldPositionUnderMouse();
            Powerup powerup = DebugUi.PowerupRow.Powerup;
            PoolManager.Instance.PickupPool.GetSpecificPowerup(mousePos, powerup).OnSpawn();
        }

        private static void TestRetribution()
        {
            Vector3 position = SpaceUtil.WorldPositionUnderMouse();
            RetributionBullet.StartRetribution(position);
        }

        public static TEnemy SpawnSpecificEnemy<TEnemy>() where TEnemy : Enemy
        {
            var ret = PoolManager.Instance.EnemyPool.SpawnSpecificEnemy<TEnemy>();
            return ret;
        }

        public static TEnemy SpawnSpecificEnemy<TEnemy>(Vector3 position) where TEnemy : Enemy
        {
            var ret = SpawnSpecificEnemy<TEnemy>();
            ret.transform.position = position;
            return ret;
        }

        private static void SpawnAllEnemies()
        {
            SpawnSpecificEnemy<BasicEnemy>();
            SpawnSpecificEnemy<CradleEnemy>();
            SpawnSpecificEnemy<RingEnemy>();
            SpawnSpecificEnemy<TankEnemy>();
            SpawnSpecificEnemy<LaserEnemy>();
        }

        private static void PowerupSuite()
        {
            var spawnTypes = new Type[]
            {
                typeof(VictimPowerup),
                typeof(OthelloPowerup),
                typeof(FireSpeedPowerup),
                typeof(SnakeBitePowerup),
                typeof(VoidPowerup)
            };

            Vector3 spawnPos = Player.Instance.transform.position;

            foreach(var powerupType in spawnTypes)
            {
                PoolManager.Instance.PickupPool.GetSpecificPowerup(spawnPos, powerupType).OnSpawn();
            }
        }

        #endregion Test Methods
    }
}