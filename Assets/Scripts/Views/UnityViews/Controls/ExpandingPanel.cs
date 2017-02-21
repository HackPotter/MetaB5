using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Metablast.UI
{
    [RequireComponent(typeof(RectTransform))]
	public class ExpandingPanel : UIBehaviour
	{
        public VerticalLayoutGroup LayoutGroup;
        public LayoutElement LayoutElement;
        public int ExpansionRatePixelsPerSecond = 100;

        protected override void Awake()
        {
            base.Awake();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                float height = (LayoutGroup.transform as RectTransform).rect.height;
                Debug.Log(height);
            }

            float currentHeight = LayoutElement.preferredHeight;
            float targetHeight = (LayoutGroup.transform as RectTransform).rect.height;

            float newHeight = Mathf.MoveTowards(currentHeight, targetHeight, ExpansionRatePixelsPerSecond * Time.deltaTime);

            LayoutElement.preferredHeight = newHeight;
            //_rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);
        }
	}
}
