using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Hierarchy.ColorHandlers
{
    public class SliderColorHandler : ColorHandler
    {
        private Slider Slider;

        public override Color Color
        {
            get => Slider.image.color;
            set
            {
                if (Slider.image.color != value)
                    Slider.image.color = value;
            }
        }

        public SliderColorHandler(Slider slider)
        {
            Slider = slider;
        }
    }
}
