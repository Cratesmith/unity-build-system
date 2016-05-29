using UnityEngine;
using System.Collections;
using System.Linq;

[ScriptExecutionOrderAttribute(-999)]
public class SkuInstance : IndexedBehaviour<SkuInstance> 
{
    static GameObject   s_skuDontDestoyInstance;
    static SkuInstance  s_current;

    protected override void OnEnable()
    {
        base.OnEnable();
        if(s_current!=null)
        {
            gameObject.SetActive(false);
        }
        else 
        {
            Init();
        }
    }

    protected override void OnDisable()
    {        
        base.OnDisable();
        if(s_current == this)
        {
            s_current = null;
            var next = All.FirstOrDefault(x=> x!=null && !x.isActiveAndEnabled);
            if(next)
            {
                next.gameObject.SetActive(false);
            }
        }

    }

	// Use this for initialization
	void Init() 
    {
        if(!SkuManager.current)
        {
            throw new System.ArgumentNullException("No current sku. Can't instantiate sku prefab");
        }

        if(SkuManager.current.skuInstancePrefab)
        {
            var instance = Instantiate(SkuManager.current.skuInstancePrefab);
            instance.transform.parent = transform;
        }

        if(!s_skuDontDestoyInstance && SkuManager.current.skuInstanceDontDestroyPrefab)
        {
            var instance = Instantiate(SkuManager.current.skuInstanceDontDestroyPrefab);
            foreach(var i in instance.GetComponentsInChildren<Transform>())
            {
                DontDestroyOnLoad(i.gameObject);
            }
            s_skuDontDestoyInstance = instance;
        }
        s_current = this;
	}
}
