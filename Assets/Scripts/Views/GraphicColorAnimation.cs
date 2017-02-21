using UnityEngine;
using UnityEngine.UI;

namespace Metablast.UI
{
    public class GraphicColorAnimation : MonoBehaviour
    {
        public Graphic Graphic;

        public Color Color;

        void OnEnable()
        {
            Color = Graphic.color;
        }
        
        void LateUpdate()
        {
            Graphic.color = Color;
        }
    }
}
