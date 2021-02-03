using Assets.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Util
{
    public static class SpaceUtil
    {
        /// <summary>
        /// A BoxMap that represents the screen measured in pixels.
        /// </summary>
        public static BoxMap ScreenMap { get; set; }

        /// <summary>
        /// A BoxMap that represents the screen measured in world space.
        /// </summary>
        public static BoxMap WorldMap { get; set; }

        /// <summary>
        /// Represents the size of the WorldMap.
        /// Used to resize elements to match the size of the current world.
        /// </summary>
        public static Vector2 WorldMapSize { get; set; }
        public static void Init()
        {
            var camera = Camera.main;

            Vector2 screenSize = new Vector2(Screen.width, Screen.height);
            ScreenMap = new BoxMap(Vector2.zero, screenSize);

            Vector2 mappedScreen = camera.ScreenToWorldPoint(screenSize);
            Vector2 mappedZero = camera.ScreenToWorldPoint(Vector2.zero);

            WorldMapSize = mappedScreen - mappedZero;

            WorldMap = new BoxMap(mappedZero, WorldMapSize);
        }


        public static void SetRightToPosition(MonoBehaviour element, Vector2 pos)
        {
            var rectTransform = (RectTransform)element.transform;
            var rect = rectTransform.rect.size;

            SetRightToPosition(element, pos, rect);
        }
        private static void SetRightToPosition(MonoBehaviour element, Vector2 pos, Vector2 elementSize)
        {
            var add = new Vector2(-elementSize.x, elementSize.y) / 2;
            var newPos = pos + add;

            element.transform.position = newPos;
        }

        public static void SetLeftToPosition(MonoBehaviour element, Vector2 pos)
        {
            var rectTransform = (RectTransform)element.transform;
            var rect = rectTransform.rect.size;

            SetLeftToPosition(element, pos, rect);
        }
        private static void SetLeftToPosition(MonoBehaviour element, Vector2 pos, Vector2 elementSize)
        {
            var add = new Vector2(elementSize.x / 2, elementSize.y / 2);
            var newPos = pos + add;

            element.transform.position = newPos;
        }

        public static void SetCenterToPosition(MonoBehaviour element, float pos)
        {
            SetCenterToPosition(element, new Vector2(pos, pos));
        }
        public static void SetCenterToPosition(MonoBehaviour element, Vector2 pos)
        {
            var rectTransform = (RectTransform)element.transform;
            var rect = rectTransform.rect.size;

            SetCenterToPosition(element, pos, rect);
        }
        private static void SetCenterToPosition(MonoBehaviour element, Vector2 pos, Vector2 elementSize)
        {
            var add = new Vector2(elementSize.x, elementSize.y) / 2;
            var newPos = pos + add;

            element.transform.position = newPos;
        }

        public static Vector2 RandomEnemySpawnPosition(Enemy enemy)
        {
            Vector2 size = ((RectTransform)enemy.transform).rect.size;

            float spawnY = WorldMap.Top.y + size.y;

            float spawnXMin = WorldMap.Left.x + size.x;
            float spawnXMax = WorldMap.Right.x - size.x;
            float spawnX = RandomUtil.Float(spawnXMin, spawnXMax);

            var ret = new Vector2(spawnX, spawnY);
            return ret;
        }








        public static Vector2 WorldPositionUnderMouse()
        {
            Vector2 mousePos = Input.mousePosition;
            var camera = Camera.main;
            Vector3 point = camera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, camera.nearClipPlane));
            return point;
        }
    }
}