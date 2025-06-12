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

    #region Math
    public static Vector2 RotateVector(Vector2 vector, float angleDegrees)
    {
        float angleRad = angleDegrees * Mathf.Deg2Rad; // 도를 라디안으로 변환
        float cos = Mathf.Cos(angleRad);
        float sin = Mathf.Sin(angleRad);

        // 2D 벡터 회전 공식 적용
        float newX = vector.x * cos - vector.y * sin;
        float newY = vector.x * sin + vector.y * cos;

        return new Vector2(newX, newY);
    }

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
    #endregion
}