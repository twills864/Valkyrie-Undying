﻿using Assets.Enemies;
using UnityEngine;

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

        /// <summary>
        /// Right-aligns a given element to a specified position.
        /// </summary>
        /// <param name="element">The element to position.</param>
        /// <param name="pos">The position to set.</param>
        public static void SetRightToPosition(MonoBehaviour element, Vector2 pos)
        {
            var rectTransform = (RectTransform)element.transform;
            var rect = rectTransform.rect.size;

            SetRightToPosition(element, pos, rect);
        }
        /// <summary>
        /// Right-aligns a given element to a specified position.
        /// </summary>
        /// <param name="element">The element to position.</param>
        /// <param name="pos">The position to set.</param>
        private static void SetRightToPosition(MonoBehaviour element, Vector2 pos, Vector2 elementSize)
        {
            var add = new Vector2(-elementSize.x, elementSize.y) / 2;
            var newPos = pos + add;

            element.transform.position = newPos;
        }

        /// <summary>
        /// Left-aligns a given element to a specified position.
        /// </summary>
        /// <param name="element">The element to position.</param>
        /// <param name="pos">The position to set.</param>
        public static void SetLeftToPosition(MonoBehaviour element, Vector2 pos)
        {
            var rectTransform = (RectTransform)element.transform;
            var rect = rectTransform.rect.size;

            SetLeftToPosition(element, pos, rect);
        }
        /// <summary>
        /// Left-aligns a given element to a specified position.
        /// </summary>
        /// <param name="element">The element to position.</param>
        /// <param name="pos">The position to set.</param>
        private static void SetLeftToPosition(MonoBehaviour element, Vector2 pos, Vector2 elementSize)
        {
            var add = new Vector2(elementSize.x / 2, elementSize.y / 2);
            var newPos = pos + add;

            element.transform.position = newPos;
        }

        /// <summary>
        /// Center-aligns a given element to a specified position.
        /// </summary>
        /// <param name="element">The element to position.</param>
        /// <param name="pos">The position to set.</param>
        public static void SetCenterToPosition(MonoBehaviour element, float pos)
        {
            SetCenterToPosition(element, new Vector2(pos, pos));
        }
        /// <summary>
        /// Center-aligns a given element to a specified position.
        /// </summary>
        /// <param name="element">The element to position.</param>
        /// <param name="pos">The position to set.</param>
        public static void SetCenterToPosition(MonoBehaviour element, Vector2 pos)
        {
            var rectTransform = (RectTransform)element.transform;
            var rect = rectTransform.rect.size;

            SetCenterToPosition(element, pos, rect);
        }
        /// <summary>
        /// Center-aligns a given element to a specified position.
        /// </summary>
        /// <param name="element">The element to position.</param>
        /// <param name="pos">The position to set.</param>
        private static void SetCenterToPosition(MonoBehaviour element, Vector2 pos, Vector2 elementSize)
        {
            var add = new Vector2(elementSize.x, elementSize.y) / 2;
            var newPos = pos + add;

            element.transform.position = newPos;
        }

        /// <summary>
        /// Generates and returns a spawn position for an enemy
        /// with a Y-coordinate just above the visible screen,
        /// and a random X-coordinate within the visible screen.
        /// </summary>
        /// <param name="enemy">The enemy that will be spawned.</param>
        /// <returns>The random spawn position for the <paramref name="enemy"/></returns>
        public static Vector2 RandomEnemySpawnPosition(Enemy enemy)
        {
            Vector2 size = ((RectTransform)enemy.transform).rect.size;

            float spawnY = WorldMap.Top.y + size.y;

            float sizeX = size.x * enemy.transform.localScale.x;
            GetWorldBoundsX(sizeX, out float spawnXMin, out float spawnXMax);
            float spawnX = RandomUtil.Float(spawnXMin, spawnXMax);

            var ret = new Vector2(spawnX, spawnY);
            return ret;
        }

        /// <summary>
        /// Gets the minimum and maximum X world-coordinate values that an object of
        /// the specified <paramref name="width"/> could have while still being
        /// fully visible on the screen.
        /// </summary>
        /// <param name="width">The width of the object.</param>
        /// <param name="minX">The minimum X world-coordinate value.</param>
        /// <param name="maxX">The maximum X world-coordinate value.</param>
        public static void GetWorldBoundsX(float width, out float minX, out float maxX)
        {
            width *= 0.5f;
            minX = WorldMap.Left.x + width;
            maxX = WorldMap.Right.x - width;
        }







        /// <summary>
        /// Returns the current world position under the cursor.
        /// </summary>
        /// <returns>The current world position under the cursor.</returns>
        public static Vector2 WorldPositionUnderMouse()
        {
            Vector2 mousePos = Input.mousePosition;
            var camera = Camera.main;
            Vector3 point = camera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, camera.nearClipPlane));
            return point;
        }
    }
}