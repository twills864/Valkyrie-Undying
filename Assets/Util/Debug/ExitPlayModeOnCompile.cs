using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
/// <summary>
/// By default, the Unity editor doesn't stop playing if the code is recompiled.
/// This breaks some functionality of the game, and this class is necessary
/// to automatically stop the player on recompiling.
/// </summary>
[InitializeOnLoad]
public static class ExitPlayModeOnCompile
{
    static ExitPlayModeOnCompile()
    {
        // Rely on the fact that recompiling causes an assembly reload and that, in turn,
        // causes our static constructor to be called again.
        if (EditorApplication.isPlaying)
        {
            Debug.Log("Recompiled, stopping!");
            //LogUtil.Log("Recompiled, stopping!");
            //Debug.LogWarning("Stopping Editor because of AssemblyReload.");
            EditorApplication.isPlaying = false;
        }
    }
}
#endif