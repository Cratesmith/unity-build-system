using UnityEngine;
using System.Collections;
using System.IO;
using System.Linq;

public class SkuManager : ResourceSingleton<SkuManager>
{
    [SerializeField] SkuTypeBase m_current;
    public static SkuTypeBase current 
    {
        get { return instance.m_current; }
#if UNITY_EDITOR
        set 
        {
            if(value == instance.m_current)
                return;

            UnityEditor.Undo.RecordObject(instance, "Changed Selected Sku");
            
            if(instance.m_current)
            {
                instance.m_current.DisablePlatform();
            }   

            instance.m_current = value;
            if(instance.m_current)
            {
                instance.m_current.EnablePlatform();
            }

            UnityEditor.EditorUtility.SetDirty(instance);
        }
#endif
    }
}
