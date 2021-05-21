using Assets.Enemies;
using UnityEngine;

namespace Assets.Util
{
    public static class SpaceUtil
    {
        /// <summary>
        /// A z-coordinate that will render below all other active gameObjects.
        /// </summary>
        public const float DeepZPosition = 999;

        /// <summary>
        /// A BoxMap that represents the screen measured in pixels.
        /// </summary>
        public static BoxMap ScreenMap { get; private set; }

        /// <summary>
        /// A BoxMap that represents the screen measured in world space.
        /// </summary>
        public static BoxMap WorldMap { get; private set; }

        /// <summary>
        /// Represents the size of the WorldMap.
        /// Used to resize elements to match the size of the current world.
        /// </summary>
        public static Vector2 WorldMapSize { get; private set; }

        public static float InverseWorldMapHeight { get; private set; }

        public static void Init()
        {
            var camera = Camera.main;

            Vector2 screenSize = new Vector2(Screen.width, Screen.height);
            ScreenMap = new BoxMap(Vector3.zero, screenSize);

            Vector3 mappedScreen = camera.ScreenToWorldPoint(screenSize);
            Vector3 mappedZero = camera.ScreenToWorldPoint(Vector2.zero);

            WorldMapSize = mappedScreen - mappedZero;

            WorldMap = new BoxMap(mappedZero, WorldMapSize);

            InverseWorldMapHeight = 1f / WorldMapSize.y;
        }

        #region Set Positions

        /// <summary>
        /// Right-aligns a given element to a specified position.
        /// </summary>
        /// <param name="element">The element to position.</param>
        /// <param name="pos">The position to set.</param>
        public static void SetRightToPosition(MonoBehaviour element, Vector3 pos)
        {
            var rectTransform = (RectTransform)element.transform;
            var rectSize = rectTransform.rect.size * element.transform.localScale.x;

            SetRightToPosition(element, pos, rectSize);
        }
        /// <summary>
        /// Right-aligns a given element to a specified position.
        /// </summary>
        /// <param name="element">The element to position.</param>
        /// <param name="pos">The position to set.</param>
        /// /// <param name="elementSize">The size of the element.</param>
        private static void SetRightToPosition(MonoBehaviour element, Vector3 pos, Vector2 elementSize)
        {
            var add = new Vector3(-elementSize.x, elementSize.y) * 0.5f;
            var newPos = pos + add;

            element.transform.position = newPos;
        }

        /// <summary>
        /// Left-aligns a given element to a specified position.
        /// </summary>
        /// <param name="element">The element to position.</param>
        /// <param name="pos">The position to set.</param>
        public static void SetLeftToPosition(MonoBehaviour element, Vector3 pos)
        {
            var rectTransform = (RectTransform)element.transform;
            var rectSize = rectTransform.rect.size * element.transform.localScale.x;

            SetLeftToPosition(element, pos, rectSize);
        }
        /// <summary>
        /// Left-aligns a given element to a specified position.
        /// </summary>
        /// <param name="element">The element to position.</param>
        /// <param name="pos">The position to set.</param>
        /// <param name="elementSize">The size of the element.</param>
        private static void SetLeftToPosition(MonoBehaviour element, Vector3 pos, Vector2 elementSize)
        {
            var add = new Vector3(elementSize.x, elementSize.y) * 0.5f;
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
            SetCenterToPosition(element, new Vector3(pos, pos));
        }
        /// <summary>
        /// Center-aligns a given element to a specified position.
        /// </summary>
        /// <param name="element">The element to position.</param>
        /// <param name="pos">The position to set.</param>
        public static void SetCenterToPosition(MonoBehaviour element, Vector3 pos)
        {
            var rectTransform = (RectTransform)element.transform;
            var rectSize = rectTransform.rect.size;

            SetCenterToPosition(element, pos, rectSize);
        }
        /// <summary>
        /// Center-aligns a given element to a specified position.
        /// </summary>
        /// <param name="element">The element to position.</param>
        /// <param name="pos">The position to set.</param>
        /// /// <param name="elementSize">The size of the element.</param>
        private static void SetCenterToPosition(MonoBehaviour element, Vector3 pos, Vector2 elementSize)
        {
            var add = new Vector3(elementSize.x, elementSize.y) * 0.5f;
            var newPos = pos + add;

            element.transform.position = newPos;
        }

        #endregion Set Positions

        /// <summary>
        /// Generates and returns a spawn position for an enemy
        /// with a Y-coordinate just above the visible screen,
        /// and a random X-coordinate within the visible screen.
        /// </summary>
        /// <param name="enemy">The enemy that will be spawned.</param>
        /// <returns>The random spawn position for the <paramref name="enemy"/></returns>
        public static Vector3 RandomEnemySpawnPosition(Enemy enemy)
        {
            Vector2 size = ((RectTransform)enemy.transform).rect.size;

            float spawnY = WorldMap.Top.y + size.y;

            float sizeX = size.x * enemy.transform.localScale.x;
            float spawnX = RandomWorldPositionX(sizeX);

            var ret = new Vector3(spawnX, spawnY);
            return ret;
        }

        /// <summary>
        /// Generates and returns a random position for an enemy
        /// to inevitably travel to.
        /// </summary>
        /// <param name="enemy">The enemy that will be spawned.</param>
        /// <returns>The random spawn position for the <paramref name="enemy"/></returns>
        public static Vector3 RandomEnemyDestinationTopHalf(Enemy enemy)
        {
            Vector2 size = ((RectTransform)enemy.transform).rect.size;

            const float TopMargin = 2.0f;
            float sizeY = size.y * enemy.transform.localScale.y;
            GetWorldBoundsYTopHalf(sizeY + TopMargin, out float spawnYMin, out float spawnYMax);
            float spawnY = RandomUtil.Float(spawnYMin, spawnYMax);

            float sizeX = size.x * enemy.transform.localScale.x;
            float spawnX = RandomWorldPositionX(sizeX);

            var ret = new Vector3(spawnX, spawnY);
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
        /// Gets the minimum and maximum Y world-coordinate values that an object of
        /// the specified <paramref name="height"/> could have while still being
        /// fully visible on the screen.
        /// </summary>
        /// <param name="height">The width of the object.</param>
        /// <param name="minY">The minimum Y world-coordinate value.</param>
        /// <param name="maxY">The maximum Y world-coordinate value.</param>
        public static void GetWorldBoundsY(float height, out float minY, out float maxY)
        {
            height *= 0.5f;
            minY = WorldMap.Bottom.y + height;
            maxY = WorldMap.Top.y - height;
        }

        /// <summary>
        /// Gets the minimum and maximum Y world-coordinate values that an object of
        /// the specified <paramref name="height"/> could have in the top half of
        /// while still being fully visible on the screen.
        /// </summary>
        /// <param name="height">The width of the object.</param>
        /// <param name="minY">The minimum Y world-coordinate value.</param>
        /// <param name="maxY">The maximum Y world-coordinate value.</param>
        public static void GetWorldBoundsYTopHalf(float height, out float minY, out float maxY)
        {
            height *= 0.5f;
            minY = WorldMap.Center.y + height;
            maxY = WorldMap.Top.y - height;
        }

        //public static float RatioOfWorldHeight(float yPosition)
        //{
        //    float height = yPosition + (WorldMapSize.y * 0.5f);
        //    float ratio = height * InverseWorldMapHeight;
        //    return ratio;
        //}

        /// <summary>
        /// Returns the distance from a specified Y position to the top of the world space.
        /// </summary>
        /// <param name="yPosition">The Y position to find the distance from.</param>
        /// <returns>The distance to the top of the world.</returns>
        public static float WorldDistanceToTop(float yPosition)
        {
            float height = WorldMap.Top.y - yPosition;
            return height;
        }

        /// <summary>
        /// Returns a random X position that an object of the specified width
        /// could have while still being fully visible on the screen.
        /// </summary>
        /// <param name="widthOfObject">The width of the object.</param>
        public static float RandomWorldPositionX(float widthOfObject = 0f)
        {
            GetWorldBoundsX(widthOfObject, out float posXMin, out float posXMax);
            float posX = RandomUtil.Float(posXMin, posXMax);
            return posX;
        }

        /// <summary>
        /// Returns a random Y position that an object of the specified height
        /// could have while still being fully visible on the screen.
        /// </summary>
        /// <param name="heightOfObject">The height of the object.</param>
        public static float RandomWorldPositionY(float heightOfObject = 0f)
        {
            GetWorldBoundsY(heightOfObject, out float posYMin, out float posYMax);
            float posY = RandomUtil.Float(posYMin, posYMax);
            return posY;
        }

        /// <summary>
        /// Determines whether or not a given world <paramref name="point"/>
        /// exists within the bounds of the world map.
        /// </summary>
        /// <param name="point">The point to check.</param>
        /// <returns>True if the point exists inside the world map; false otherwise.</returns>
        public static bool PointIsInBounds(Vector3 point)
        {
            bool inside = SpaceUtil.WorldMap.ContainsPoint(point);
            return inside;
        }


        /// <summary>
        /// Returns the current world position under the cursor.
        /// </summary>
        /// <returns>The current world position under the cursor.</returns>
        public static Vector3 WorldPositionUnderMouse()
        {
            Vector3 mousePos = Input.mousePosition;
            var camera = Camera.main;
            Vector3 point = camera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, camera.nearClipPlane));
            point.z = 0;
            return point;
        }

        /// <summary>
        /// Attempts to get a game object directly underneath the current position of the cursor.
        /// </summary>
        /// <param name="gameObject">The GameObject (if any) under the cursor.</param>
        /// <returns>True if a GameObject exists under the cursor; false otherwise.</returns>
        public static bool TryGetGameObjectUnderMouse(out GameObject gameObject)
        {
            var mousePos = WorldPositionUnderMouse();
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            gameObject = hit.collider?.gameObject;

            bool ret = gameObject != null;
            return ret;
        }

        /// <summary>
        /// Attempts to get an enemy directly underneath the current position of the cursor.
        /// </summary>
        /// <param name="enemy">The enemy (if any) under the cursor.</param>
        /// <returns>True if an enemy exists under the cursor; false otherwise.</returns>
        public static bool TryGetEnemyUnderMouse(out Enemy enemy)
        {
            bool ret;

            if(TryGetGameObjectUnderMouse(out GameObject gameObject))
                ret = gameObject.TryGetComponent<Enemy>(out enemy);
            else
            {
                enemy = null;
                ret = false;
            }

            return ret;
        }

        #region Pan

        public static float PanFromPosition(Vector3 position)
        {
            return PanFromPosition(position.x);
        }

        public static float PanFromPosition(float x)
        {
            float offsetFromCenter = x - SpaceUtil.WorldMap.Center.x;
            float widthHalf = SpaceUtil.WorldMapSize.x * 0.5f;

            float pan = offsetFromCenter / widthHalf;
            return pan;
        }

        #endregion Pan
    }
}