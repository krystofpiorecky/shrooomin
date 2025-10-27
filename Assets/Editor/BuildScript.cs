using UnityEditor;
using UnityEditor.Build.Reporting;
using System.IO;

public static class BuildScript
{
  // Adds a button under "Build → Build Linux Server" in the Unity menu bar
  [MenuItem("Build/Build Linux Server")]
  public static void BuildLinuxServer()
  {
    string buildPath = "Builds/LinuxServer";
    Directory.CreateDirectory(buildPath);

    string[] scenes = { "Assets/Scenes/MainScene.unity" }; // update this!

    BuildPlayerOptions opts = new BuildPlayerOptions
    {
      scenes = scenes,
      locationPathName = Path.Combine(buildPath, "MyGameServer.x86_64"),
      target = BuildTarget.StandaloneLinux64,
      subtarget = (int)StandaloneBuildSubtarget.Server,
      options = BuildOptions.None
    };

    var report = BuildPipeline.BuildPlayer(opts);
    var summary = report.summary;

    UnityEngine.Debug.Log($"Build result: {summary.result}");
    UnityEngine.Debug.Log($"Platform: {summary.platform}");
    UnityEngine.Debug.Log($"Output: {summary.outputPath}");
    UnityEngine.Debug.Log($"Errors: {summary.totalErrors}");
    UnityEngine.Debug.Log($"Warnings: {summary.totalWarnings}");

    foreach (var step in report.steps)
    {
      foreach (var message in step.messages)
      {
        UnityEngine.Debug.LogError(message.content);
      }
    }
  }
}
