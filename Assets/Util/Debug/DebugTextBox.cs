using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.Util.AssetsDebug
{
    public class DebugTextBox
    {
        private InputField InputField { get; set; }
        private Image Image;

        private Color ColorSuccess => new Color(1f, 1f, 1f);
        private Color ColorFailure => new Color(1f, 0.5f, 0.5f);

        public string Value => InputField.text;

        public DebugTextBox(InputField inputField)
        {
            InputField = inputField;
            Image = InputField.GetComponent<Image>();
        }

        private void OnSuccess()
        {
            Image.color = ColorSuccess;
        }
        private void OnFailure()
        {
            Image.color = ColorFailure;
        }


        delegate bool TryParseDelegate<T>(string input, out T output);
        private T Get<T>(TryParseDelegate<T> tryParse, T defaultValue = default)
        {
            if(tryParse(Value, out T ret))
            {
                OnSuccess();
                return ret;
            }
            else
            {
                OnFailure();
                return defaultValue;
            }
        }

        public int GetInt(int defaultValue = default)
        {
            return Get(int.TryParse, defaultValue);
        }

        public float GetFloat(float defaultValue = default)
        {
            return Get(float.TryParse, defaultValue);
        }

        public double GetDouble(double defaultValue = default)
        {
            return Get(double.TryParse, defaultValue);
        }
    }
}