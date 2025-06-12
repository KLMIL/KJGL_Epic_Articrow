using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BMC;
using TMPro;
using YSJ;

public class ResourceManager
{
    #region 캐싱
    public TextMeshPro DamageText { get; private set; } // 데미지 텍스트

    #endregion

    public void Init()
    {
        // 데미지 텍스트
        DamageText = Managers.Resource.Load<TextMeshPro>("Text/DamageText");
    }

    // Resources 폴더 내에 있는 리소스 반환
    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    // Resources 폴더 내의 특정 폴더의 전체 리소스 반환
    public T[] LoadAll<T>(string path) where T : Object
    {
        return Resources.LoadAll<T>(path);
    }

    // Resources의 Prefabs 폴더 내의 특정 리소스 인스턴스화
    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{path}");
        if (prefab == null)
        {
            return null;
        }
        return Object.Instantiate(prefab, parent);
    }

    // 인스턴스 파괴
    public void Destroy(GameObject go)
    {
        if (go == null)
            return;
        Object.Destroy(go);
    }
}