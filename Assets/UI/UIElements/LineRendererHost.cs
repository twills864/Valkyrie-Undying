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
    /// <summary>
    /// Holds a Unity LineRenderer. Useful for when a GameObject needs
    /// multiple LineRenderer components, which Unity does not support.
    /// </summary>
    /// <inheritdoc/>
    public class LineRendererHost : UIElement
    {
        #region Prefabs

        [SerializeField]
        private LineRenderer _Line = null;

        #endregion Prefabs


        #region Prefab Properties

        public LineRenderer Line => _Line;

        #endregion Prefab Properties


        protected override ColorHandler DefaultColorHandler()
        {
            return new LineRendererColorHandler(_Line);
        }
    }
}
