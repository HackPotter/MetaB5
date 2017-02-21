using UnityEngine;
using UnityEngine.UI;

namespace Metablast.UI
{
    public class ContextMessageView : MonoBehaviour, IContextMessageView
    {
#pragma warning disable 0067, 0649
        [SerializeField]
        private Image myImage;
        [SerializeField]
        private Text myText;
        [SerializeField]
        private Color _defaultBackgroundColor;
#pragma warning restore 0067, 0649

        private Color _targetBackgroundColor;
        private Color _currentBackgroundColor;
        
        private string _lpt = ""; //Low-Priority Text
        private string _text = "";
        
        public void SetLowPriorityText(string text)
        {
            _lpt = text;
            SetText();
        }

        public void SetText(string text)
        {
            _text = text;
            _currentBackgroundColor = _defaultBackgroundColor;
            SetText();
        }

        private void SetText()
        {
            if (!string.IsNullOrEmpty(_text))
            {
                myText.text = _text;
                _targetBackgroundColor = _currentBackgroundColor;
                myImage.enabled = true;
                gameObject.SetActive(true);
            }
            else if (!string.IsNullOrEmpty(_lpt))
            {
                myText.text = _lpt;
                _targetBackgroundColor = _defaultBackgroundColor;
                myImage.enabled = true;
                gameObject.SetActive(true);
            }
            else
            {
                _targetBackgroundColor.a = 0;
                myText.text = "";
            }
        }

        void Update()
        {
            myImage.color = Color.Lerp(myImage.color, _targetBackgroundColor, 0.1f);
            if (myImage.color.a < 0.01f)
            {
                gameObject.SetActive(false);
            }
        }

        public void SetText(string text, Color color)
        {
            _text = text;
            _currentBackgroundColor = color;
            SetText();
        }
    }
}
