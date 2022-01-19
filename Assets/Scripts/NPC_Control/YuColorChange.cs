using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yu_Controller
{
    public class YuColorChange : MonoBehaviour
    {
        [SerializeField] private Material material;
        [SerializeField] private Material materialCopy;
        [SerializeField] private Light yuLight;
        private int colorID;
        /// color array is placeholders :)
        private Color[] colors = { new Color(1, 0.6862745f, 0), Color.blue, Color.red, Color.green, Color.magenta };
        private Color targetColor;
        private Color currentColor;

        void Start()
        {
            colorID = Shader.PropertyToID("_RimColor");
            ///material copy is so that only one material needs to be changed if properties are changed :)
            materialCopy.CopyPropertiesFromMaterial(material);
            currentColor = colors[0];
            targetColor = colors[0];
            if (yuLight != null) { yuLight.color = currentColor; }
        }

        void Update()
        {
            ///interpolates between the target color and current color
            if (currentColor != targetColor)
            {
                currentColor = Color.Lerp(currentColor, targetColor, 1.0f / 25.0f);
                materialCopy.SetColor(colorID, currentColor);
                ///incase we need it without a light, idk
                if (yuLight != null) { yuLight.color = currentColor; }
            }
        }

        ///<summary>Sets the color of Yu based on the inputed color</summary>
        public void SetColor(Color color)
        {
            targetColor = color;
        }

        ///<summary>Sets the color of Yu based on the inputed index for the colors array</summary>
        public void SetColor(int index)
        {
            targetColor = colors[index];
        }

        ///<summary>Gets the color of yu. will probably go unused. :D </summary>
        public Color GetColor()
        {
            return currentColor;
        }
    }
}
