using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Hierarchy.ColorHandlers
{
    public class TextMeshColorHandler : ColorHandler
    {
        private TextMesh Mesh;

        public override Color Color
        {
            get => Mesh.color;
            set
            {
                if (Mesh.color != value)
                    Mesh.color = value;
            }
        }

        public TextMeshColorHandler(TextMesh mesh)
        {
            Mesh = mesh;
        }
    }
}
