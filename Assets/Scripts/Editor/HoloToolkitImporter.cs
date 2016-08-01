using UnityEngine;
using UnityEditor;

using System.IO;
using System.Collections;

namespace HoloLensUnitySampler.Editor
{
    public static class HoloToolkitImporter
    {
        private static object locker = new object();
        private static bool exported;

        private static readonly string batchFilePath = CombinePath(Application.dataPath, "..", "Tools", "exportHoloToolKit.bat"); // Path to batch file 
        private static readonly string unityAppPath = EditorApplication.applicationPath; // Path to Unity.exe
        private static readonly string toolkitPath = CombinePath(Application.dataPath, "..", "HoloToolkit-Unity"); // Path to HoloToolkit-Unity project root
        private static readonly string exportPath = CombinePath(Application.dataPath, "..", "Tools", "HoloToolkitUnity.unitypackage"); // Path to exported .unitypackage of HoloToolkit-Unity 

        [MenuItem("Tools/Import HoloToolkit")]
        public static void ImportHoloToolkit()
        {
            var process = new System.Diagnostics.Process();
            process.StartInfo.FileName = batchFilePath;
            process.StartInfo.Arguments = BuildArguments(unityAppPath, toolkitPath, exportPath);
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.EnableRaisingEvents = true;
            process.Exited += (sender, e) =>
            {
                lock(locker)
                {
                    exported = true;
                }
            };
            process.Start();

            lock (locker)
            {
                exported = false;
            }

            EditorApplication.update += Update;
            EditorUtility.DisplayProgressBar("Hold on", "Exporting HoloToolkit project to custom package", 1);
        }

        private static void Update()
        {
            lock(locker)
            {
                if(exported)
                {
                    EditorUtility.ClearProgressBar();
                    AssetDatabase.ImportPackage(exportPath, false);
                    EditorApplication.update -= Update;
                }
            }
        }

        #region Util

        private static string CombinePath(params string[] paths)
        {
            if (paths == null) { return null; }

            string combinedPath = "";

            foreach (var path in paths)
            {
                if (path != null)
                {
                    combinedPath = Path.Combine(combinedPath, path);
                }
            }

            return combinedPath;
        }

        private static string BuildArguments(params string[] args)
        {
            if (args == null) { return null; }

            var sb = new System.Text.StringBuilder();

            foreach (var arg in args)
            {
                if(arg != null)
                {
                    sb.AppendFormat("\"{0}\" ", arg); // Separate arguments by space
                }
            }

            sb.Remove(sb.Length - 1, 1); // Remove last space

            return sb.ToString();
        }

        #endregion
    }
}

