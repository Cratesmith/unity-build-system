using UnityEngine;
using System.Collections;

public abstract class SkuTypeBase : ResourceSingleton<SkuTypeBase>
{
#if UNITY_EDITOR
    public virtual void EnablePlatform()        {}
    public virtual void DisablePlatform()       {} 
#endif

    [SerializeField] GameObject m_skuInstancePrefab;
    [SerializeField] GameObject m_skuInstanceDontDestroyPrefab;
    public GameObject skuInstancePrefab            { get { return m_skuInstancePrefab; } }
    public GameObject skuInstanceDontDestroyPrefab { get { return m_skuInstanceDontDestroyPrefab; } }
}