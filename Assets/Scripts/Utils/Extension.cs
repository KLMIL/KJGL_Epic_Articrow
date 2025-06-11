using System.Collections.Generic;
using UnityEngine;

// 확장 메서드 클래스
public static class Extension
{
    #region 직계 자식에서 컴포넌트 찾기
    // 직계 자식들 중에서 컴포넌트 찾는 메서드
    public static T GetComponentInDirectChildren<T>(this UnityEngine.GameObject go) where T : UnityEngine.Component
    {
        foreach (UnityEngine.Transform child in go.transform)
        {
            if (child.TryGetComponent<T>(out T component))
            {
                return component;
            }
        }
        return null;
    }

    // 직계 자식들 중에서 컴포넌트들을 찾는 메서드
    public static T[] GetComponentsInDirectChildren<T>(this UnityEngine.GameObject go) where T : UnityEngine.Component
    {
        List<T> componentList = new List<T>();
        foreach (UnityEngine.Transform child in go.transform)
        {
            if (child.TryGetComponent<T>(out T component))
            {
                componentList.Add(component);
            }
        }
        return (componentList.Count != 0) ? componentList.ToArray() : null;
    }
    #endregion
}
