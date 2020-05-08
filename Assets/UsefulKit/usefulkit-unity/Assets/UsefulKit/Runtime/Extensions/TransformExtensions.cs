using UnityEngine;

public static class TransformExtensions
{
    public static void SetActiveChildren(this Transform t, bool activeState)
    {
        for (int i = 0; i < t.childCount; i++)
        {
            t.GetChild(i).gameObject.SetActive(activeState);
        }
    }
    public static T GetComponentInDirectChildren<T>(this GameObject t)
    {
        for (var i = 0; i < t.transform.childCount; i++)
        {
            var component = t.transform.GetChild(i).GetComponent<T>();
            if(component != null) return component;
        }
        return default(T);
    }
}
