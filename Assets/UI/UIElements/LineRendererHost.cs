using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Hierarchy.ColorHandlers;
using Assets.Util;
using UnityEngine;

namespace Assets.UI
{
    public class LineRendererHost : UIElement
    {
        protected override ColorHandler DefaultColorHandler()
        {
            return new LineRendererColorHandler(_Line);
        }

        [SerializeField]
        private LineRenderer _Line;

        public LineRenderer Line => _Line;
    }
}
