using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System.Collections;

using HoloToolkit.Unity;

namespace HoloLensUnitySampler
{
    public class InfoWindow : Singleton<InfoWindow>
    {
        [SerializeField]
        private GameObject infoTextPanel;

        [SerializeField]
        private Text infoText;

        public string InfoText { set { infoText.text = value; } }

        public void OnLauncerButtonClick()
        {
            SceneManager.LoadScene("Launcher");
        }

        public void OnInfoButtonClick()
        {
            infoTextPanel.SetActive(!infoTextPanel.activeSelf);
        }
    }
}
