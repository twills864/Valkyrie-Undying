﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.ColorManagers;
using Assets.GameTasks;
using Assets.Hierarchy.ColorHandlers;
using Assets.Util;
using UnityEngine;

namespace Assets.UI
{
    public class MortarGuide : UIElement
    {
        public static MortarGuide Instance { get; set; }

        protected override ColorHandler DefaultColorHandler()
        {
            return new NullColorHandler();
        }

        [SerializeField]
        private float GuideWidth;
        [SerializeField]
        private float FadeInTime;

        [SerializeField]
        private LineRendererHost LeftHost;
        [SerializeField]
        private LineRendererHost RightHost;

        private LineRenderer Left { get; set; }
        private LineRenderer Right { get; set; }

        private void Awake()
        {
            Instance = this;
        }

        public static void StaticInit(in ColorManager colorManager)
        {
            Instance.InitInstance(in colorManager);
        }

        private void InitInstance(in ColorManager colorManager)
        {
            Instance.PositionY = SpaceUtil.WorldMap.Center.y;

            LeftHost.Init();
            RightHost.Init();

            Left = LeftHost.Line;
            Right = RightHost.Line;

            Left.SetPosition(0, SpaceUtil.WorldMap.BottomLeft);
            Right.SetPosition(0, SpaceUtil.WorldMap.BottomRight);

            Left.startWidth = GuideWidth;
            Left.endWidth = GuideWidth;
            Right.startWidth = GuideWidth;
            Right.endWidth = GuideWidth;

            var left = Instance.LeftHost.ColorHandler;
            var right = Instance.RightHost.ColorHandler;
            ColorHandler = new CollectionColorHandler(left, right);

            Color guideColor = colorManager.DefaultPlayer;
            guideColor.a = colorManager.UI.MortarGuideAlpha;
            SpriteColor = guideColor;

            gameObject.SetActive(false);
        }

        protected override void OnActivate()
        {
            FadeTo fadeIn = new FadeTo(this, 0f, Alpha, FadeInTime);
            RunTask(fadeIn);
        }

        public void DrawMortar()
        {
            Color color = SpriteColor;
            color.a = 0.5f;

            var map = SpaceUtil.WorldMap;
            Vector3 center = new Vector3(PositionX, map.Center.y, 0);

            Vector3 leftDiff = center - map.BottomLeft;
            Left.SetPosition(1, center + leftDiff);

            Vector3 rightDiff = center - map.BottomRight;
            Right.SetPosition(1, center + rightDiff);
        }
    }
}
