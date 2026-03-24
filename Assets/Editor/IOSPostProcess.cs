using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.IO;

public static class IOSPostprocess
{
    [PostProcessBuild(999)]
    public static void OnPostprocessBuild(BuildTarget target, string path)
    {
        if (target != BuildTarget.iOS) return;

        string projPath = PBXProject.GetPBXProjectPath(path);
        PBXProject proj = new PBXProject();
        proj.ReadFromFile(projPath);

#if UNITY_2019_3_OR_NEWER
        string targetGuid = proj.GetUnityMainTargetGuid();
#else
        string targetGuid = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
#endif

        string filePath = "Libraries/Plugins/iOS/InstallTime.mm";
        string fileGuid  = proj.AddFile(filePath, filePath, PBXSourceTree.Source);

        proj.AddFileToBuild(targetGuid, fileGuid);
        proj.WriteToFile(projPath);
    }
}
