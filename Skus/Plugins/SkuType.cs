using UnityEngine;
using System.Collections;

public abstract class SkuType<T> : SkuTypeBase where T:SkuType<T>
{
    public static T current { get { return SkuManager.current as T; } }
}
