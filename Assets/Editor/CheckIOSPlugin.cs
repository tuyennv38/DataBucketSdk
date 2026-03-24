using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using System.IO;

public class CheckIOSPlugin : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
        if (report.summary.platform != BuildTarget.iOS)
            return;

        string path = "Assets/Plugins/iOS/InstallTime.mm";
        if (!File.Exists(path))
        {
            Debug.LogError($"[SDK ERROR] Missing plugin file: {path}");
            throw new BuildFailedException("[SDK] Build failed: Missing iOS native plugin InstallTime.mm.");
        }
    }
}
