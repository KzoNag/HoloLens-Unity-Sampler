using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

namespace HoloLensUnitySampler
{
    public class LauncherWindowSource : ScriptableObject, ISerializationCallbackReceiver
    {
        public class Item
        {
            public string label;
            public string scenePath;
            public Item parent;
            public List<Item> children = new List<Item>();

            public Item(string label, string scenePath, Item parent)
            {
                this.label = label;
                this.scenePath = scenePath;
                this.parent = parent;
            }

            public static Item RootItem
            {
                get { return new Item("Features", null, null); }
            }

            public Item FindChild(string label)
            {
                return children.FirstOrDefault(_child => _child.label == label);
            }

            public Item AddChild(string label, string scenePath)
            {
                var child = new Item(label, scenePath, this);
                children.Add(child);
                return child;
            }
        }

        public Item RootItem { get; private set; }

        [SerializeField]
        private List<string> scenePathList;

        public void UpdateItem(string[] scenePaths)
        {
            RootItem = Item.RootItem;

            string prefix = "Assets/Scenes/Features";

            scenePathList.Clear();

            foreach(var path in scenePaths.OrderBy(_path => _path))
            {
                if (!path.StartsWith(prefix)) { continue; }

                scenePathList.Add(path);

                string targetPath = path.Substring(prefix.Length + 1);

                UpdateOrAddItem(RootItem, targetPath, path);
            }
        }

        public void UpdateOrAddItem(Item item, string path, string scenePath)
        {
            string[] parts = path.Split(new char[] { '/' }, 2);
            Item child = item.FindChild(parts[0]);
            if(child == null)
            {
                child = item.AddChild(parts[0], scenePath);
            }

            if(parts.Length >= 2 && !string.IsNullOrEmpty(parts[1]))
            {
                UpdateOrAddItem(child, parts[1], scenePath);
            }
        }

        #region Serialize

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            UpdateItem(scenePathList.ToArray());
        }

        #endregion
    }
}
