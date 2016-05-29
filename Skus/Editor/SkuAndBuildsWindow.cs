using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Linq;
using UBS;

[InitializeOnLoad]
public class SkuAndBuildsWindow : EditorWindow 
{
    static SkuTypeBase[]        platforms;
    static Editor               skuEditor;
    static Editor[]             ubsEditors;
    [SerializeField] Vector2    scroll;

    [MenuItem("File/Skus and Builds")]
    static void Create()
    {
        ScanProject();
        var w = GetWindow<SkuAndBuildsWindow>();   
        w.titleContent = new GUIContent("Sku & Builds");
    }

    static SkuAndBuildsWindow()
    {
        EditorApplication.delayCall += ScanProject;
        EditorApplication.projectWindowChanged += ScanProject;
    }
        
    static void ScanProject()
    {
        var assetDir = new DirectoryInfo("Assets/");
        var basePath = new DirectoryInfo(".").FullName;
        var files = assetDir.GetFiles("*.asset", SearchOption.AllDirectories).Select(x=>x.FullName.Substring(basePath.Length+1)).ToArray();
        platforms = files.Select(x=>AssetDatabase.LoadAssetAtPath<SkuTypeBase>(x))
                    .Where(x=> x!=null)
                    .ToArray();

        var buildCollections = files.Select(x=>AssetDatabase.LoadAssetAtPath<BuildCollection>(x))
                    .Where(x=> x!=null)
                    .ToArray();
          
        ubsEditors = buildCollections.Select(x=>Editor.CreateEditor( x ))
                    .ToArray();

        skuEditor = (SkuManager.current!=null) ? Editor.CreateEditor(SkuManager.current):null;
    }

    void OnGUI_EditorSettings()
    {
        GUILayout.Label("Editor Settings");
        GUILayout.BeginVertical("box");

        if(platforms!=null && platforms.Length>0)
        {
            var currentPlatformId   = System.Array.FindIndex(platforms, x=> x==SkuManager.current)+1;
            var platformStrings     = new [] {"NONE"}.Concat(platforms.Select(x=>x.name)).ToArray();
            var newPlatformId       = EditorGUILayout.Popup("VR Platform", currentPlatformId, platformStrings);
            if(newPlatformId != currentPlatformId)
            {
                if(newPlatformId==0)
                {
                    SkuManager.current = null;   
                }
                else 
                {
                    SkuManager.current = platforms[newPlatformId-1];
                }
                skuEditor = (SkuManager.current!=null) ? Editor.CreateEditor(SkuManager.current):null;
            }
        }

        EditorGUILayout.ObjectField(SkuManager.current, typeof(SkuTypeBase), false);
        GUILayout.BeginVertical("box");
        GUI.enabled = false;
        GUI.enabled = true;
        if(skuEditor!=null)
        {
            skuEditor.DrawDefaultInspector();
        }
        GUILayout.EndVertical();

        GUILayout.EndVertical();        
    }

    void OnGUI_Builds()
    {
        if(ubsEditors==null)
        {
            return;
        }
        
        GUILayout.Label("Builds");
        GUILayout.BeginVertical("box");
        foreach(var i in ubsEditors)
        {
            GUILayout.Label(i.target.name);
            GUILayout.BeginVertical("box");
            i.OnInspectorGUI();
            GUILayout.EndVertical();
        }
        GUILayout.EndVertical();
    }

    void OnGUI()
    {
        scroll = GUILayout.BeginScrollView(scroll);

        OnGUI_EditorSettings();

        GUILayout.Space(10);
        GUILayout.FlexibleSpace();

        OnGUI_Builds();       

        GUILayout.EndScrollView();
    }
}
