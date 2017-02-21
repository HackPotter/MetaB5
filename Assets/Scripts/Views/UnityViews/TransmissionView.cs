using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Metablast.UI
{
    public class TransmissionView : MonoBehaviour, ITransmissionView
    {
#pragma warning disable 0067, 0649
        [SerializeField]
        private Text _sender;
        [SerializeField]
        private Text _transmissionText;
        [SerializeField]
        private Image _portraitImage;
#pragma warning restore 0067, 0649

        public void ShowTransmission(string sender, string text, Sprite sprite, float duration)
        {
            this.StopAllCoroutines();

            _sender.text = sender;
            _transmissionText.text = text;
            _portraitImage.sprite = sprite;
            _portraitImage.SetLayoutDirty();

            _portraitImage.enabled = sprite != null;

            if (string.IsNullOrEmpty(text))
            {
                gameObject.SetActive(false);
                return;
            }
            gameObject.SetActive(true);
            StartCoroutine(TransmissionExpire(duration));
        }

        IEnumerator TransmissionExpire(float duration)
        {
            yield return new WaitForSeconds(duration);
            _transmissionText.text = "";
            _portraitImage.sprite = null;
            gameObject.SetActive(false);
        }
    }
}
