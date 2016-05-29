using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(SkuTypeBase), true)]
public class SkuTypeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var platform = target as SkuTypeBase;
        if(GUILayout.Button("Set As Active Platform"))
        {
            SkuManager.current = platform;
        }
        base.OnInspectorGUI();
    }
}
