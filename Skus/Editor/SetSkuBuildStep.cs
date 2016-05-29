using UnityEngine;
using System.Collections;
using UBS;
using UnityEditor;
using System.IO;
using System.Linq;

public class SetSkuBuildStep : IBuildStepProvider {

    public void BuildStepStart(BuildConfiguration pConfiguration)
    {
        var sku = AssetDatabase.LoadAssetAtPath<SkuTypeBase>(pConfiguration.Params);
        if(!sku)
        {
            var files = new[] {pConfiguration.Params, pConfiguration.Params+".asset"}
                .SelectMany(x=> Directory.GetFiles("Assets", x, SearchOption.AllDirectories))
                .ToArray();
            
            sku = files.Select(x=> AssetDatabase.LoadAssetAtPath<SkuTypeBase>(x))
                .FirstOrDefault(x=>x!=null);
                
            if(!sku)
            {
                Debug.LogError("No SKU asset named or at path "+pConfiguration.Params);
                return;
            }
        }

        SkuManager.current = sku;
    }

    public bool IsBuildStepDone()
    {
        return true;
    }

    public void BuildStepUpdate()
    {
    }
}
