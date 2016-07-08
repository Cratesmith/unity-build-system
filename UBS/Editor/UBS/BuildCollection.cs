using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace UBS
{
	[Serializable]
	public class BuildCollection : ScriptableObject
	{
		public BuildCollection()
		{
			version = BuildVersion.Load();

		}

		public List<BuildProcess> mProcesses = new List<BuildProcess>();

		public void SaveVersion()
		{
			version.Save();
			UnityEditor.PlayerSettings.Android.bundleVersionCode = version.revision;
			UnityEditor.PlayerSettings.bundleVersion = version.ToString();
			#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_4 || UNITY_5_6 || UNITY_5_7 || UNITY_5_8 || UNITY_5_9
			UnityEditor.PlayerSettings.WSA.packageVersion = version;
			#else
			UnityEditor.PlayerSettings.Metro.packageVersion = version;
			#endif
		}

		public BuildVersion version = null;
		public string versionCode = "1";

		public void SaveVersionCode()
		{
			UnityEditor.PlayerSettings.Android.bundleVersionCode = int.Parse(versionCode);
		}

        public BuildCollection CreateCurrentSceneClone()
        {                        
            var temp = BuildCollection.Instantiate(this);
            foreach(var i in temp.mProcesses)
            {
                i.mScenes = UnityEditor.SceneManagement.EditorSceneManager.GetAllScenes().Select(x=>x.path).ToList();
            }
            UnityEditor.AssetDatabase.CreateAsset(temp, "Assets/temp_build.asset");
            return temp;
        }
	}
}

