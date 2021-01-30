using UnityEngine;
using UnityEditor;
using LogUtilAssets;

[InitializeOnLoad]
public static class ExitPlayModeOnCompile
{
    static ExitPlayModeOnCompile()
    {
        // Rely on the fact that recompiling causes an assembly reload and that, in turn,
        // causes our static constructor to be called again.
        if (EditorApplication.isPlaying)
        {
            LogUtil.Log("Recompiled, stopping!");
            //Debug.LogWarning("Stopping Editor because of AssemblyReload.");
            EditorApplication.isPlaying = false;
        }
    }
}