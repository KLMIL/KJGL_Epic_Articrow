using System;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    #region Enum 변환
    // string -> enum
    public static T StringToEnum<T>(string str)
    {
        return (T)Enum.Parse(typeof(T), str);
    }

    // int -> enum
    public static T IntToEnum<T>(int i)
    {
        return (T)(object)i;    // 모든 자료형은 object를 상속받으므로
    }
    #endregion

    /// <summary>
    /// 각도를 기준으로 원의 둘레를 구하는 메서드
    /// </summary>
    /// <param name="radius"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static Vector3 GetPositionFromAngle(float radius, float angle)
    {
        Vector3 position = Vector3.zero;

        angle = DegreeToRadian(angle);

        position.x = Mathf.Cos(angle) * radius;
        position.y = Mathf.Sin(angle) * radius;

        return position;
    }

    /// <summary>
    /// Degree 값을 Radian 값으로 변환
    /// 1도는 "PI/180" radian
    /// angle도는 "PI/180 * angle" radian 
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static float DegreeToRadian(float angle)
    {
        return MathF.PI * angle / 180;
    }

    #region Extension 메서드
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

    #endregion
}