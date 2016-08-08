using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Linq;

namespace HoloLensUnitySampler.Editor
{
    [CustomEditor(typeof(LauncherWindowSource))]
    public class LauncerWindowSourceEditor : UnityEditor.Editor
    {
        [MenuItem("Assets/Create/Custom/LauncerWindowSource")]
        public static void Create()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if(string.IsNullOrEmpty(path))
            {
                path = "Assets";
            }
            else if(!AssetDatabase.IsValidFolder(path))
            {
                path = Path.GetDirectoryName(path);
            }
            path = string.Format("{0}/LauncherWindowSource.asset", path);
            path = AssetDatabase.GenerateUniqueAssetPath(path);

            var instance = CreateInstance<LauncherWindowSource>();
            AssetDatabase.CreateAsset(instance, path);

            UpdateSourceItem(instance);

            Selection.activeObject = instance;
        }

        override public void OnInspectorGUI()
        {
            base.DrawDefaultInspector();

            if(GUILayout.Button("Update"))
            {
                UpdateSourceItem((LauncherWindowSource)target);
            }
        }

        private static void UpdateSourceItem(LauncherWindowSource source)
        {
            var activeScenePaths = EditorBuildSettings.scenes.Where(_scene => _scene.enabled).Select(_scene => _scene.path).ToArray();
            source.UpdateItem(activeScenePaths);

            EditorUtility.SetDirty(source);
            AssetDatabase.SaveAssets();
        }
    }
}
