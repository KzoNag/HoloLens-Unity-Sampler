using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace HoloLensUnitySampler
{
    public class LauncherWindow : MonoBehaviour
    {
        [SerializeField]
        private Text titleText;

        [SerializeField]
        private Button backButton;

        [SerializeField]
        private LauncherWindowCell prototypeCell;

        [SerializeField]
        private LauncherWindowSource source;
        private LauncherWindowSource.Item currentItem;

        private List<LauncherWindowCell> cellList = new List<LauncherWindowCell>();

        void Start()
        {
            UpdateList(source.RootItem);
        }

        private void UpdateList(LauncherWindowSource.Item item)
        {
            foreach(var cell in cellList)
            {
                Destroy(cell.gameObject);
            }
            cellList.Clear();

            currentItem = item;

            foreach(var child in currentItem.children)
            {
                var cell = (LauncherWindowCell)Instantiate(prototypeCell, prototypeCell.transform.parent);

                cell.gameObject.SetActive(true);
                cell.GetComponentInChildren<Text>().text = child.label;
                cell.Item = child;
                cell.CellClicked += OnCellClick;

                cellList.Add(cell);
            }

            titleText.text = currentItem.label;

            backButton.interactable = (currentItem.parent != null);
            backButton.GetComponentInChildren<Text>().text = backButton.interactable ? "<" : "◆";
        }

        private void OnCellClick(LauncherWindowSource.Item item)
        {
            if (item.children != null && item.children.Count > 0)
            {
                UpdateList(item);
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(item.sceneName);
            }
        }

        public void OnBackButtonClick()
        {
            UpdateList(currentItem.parent);
        }
    }
}
