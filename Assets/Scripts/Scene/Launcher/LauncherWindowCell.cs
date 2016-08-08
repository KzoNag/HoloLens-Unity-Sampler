using UnityEngine;
using UnityEngine.Events;

using System.Collections;

namespace HoloLensUnitySampler
{
    public class LauncherWindowCell : MonoBehaviour
    {
        public event System.Action<LauncherWindowSource.Item> CellClicked;
        public LauncherWindowSource.Item Item;

        public void OnClick()
        {
            CellClicked(Item);
        }
    }
}
