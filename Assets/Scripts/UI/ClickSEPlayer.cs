using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System;

namespace HoloLensUnitySampler
{
    public class ClickSEPlayer : MonoBehaviour, IPointerClickHandler
    {
        public string audioEventName = "Decide";

        public void OnPointerClick(PointerEventData eventData)
        {
            bool play = true;

            var selectable = GetComponent<Selectable>();
            if (selectable != null)
            {
                play = selectable.interactable;
            }

            if (play)
            {
                HoloToolkit.Unity.UAudioManager.Instance.PlayEvent(audioEventName);
            }
        }
    }
}
