using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 풀링한 것들을 관리하는 클래스
/// </summary>
[System.Serializable]
public class Pool : MonoBehaviour
{
    public GameObject Prefab { get; private set; }                    // 풀링할 오브젝트
    [SerializeField] List<GameObject> _pool = new List<GameObject>(); // 풀링된 오브젝트 리스트
    [SerializeField] int _addCount = 1000;                              // 부족할 때 추가할 오브젝트 개수

    public void Init<T>(T prefab) where T : Object
    {
        Prefab = prefab as GameObject;
        Prepare();
    }

    // 준비
    void Prepare()
    {
        //for (int i = 0; i < _addCount; i++)
        //{
        //    GameObject poolingObject = Instantiate(Prefab, transform);
        //    poolingObject.SetActive(false);
        //    Push(poolingObject);
        //}
        StartCoroutine(Spawn()); // 코루틴으로 생성
    }

    IEnumerator Spawn()
    {
        for (int i = 0; i < _addCount; i++)
        {
            GameObject poolingObject = Instantiate(Prefab, transform);
            poolingObject.SetActive(false);
            Push(poolingObject);
            yield return null; // 프레임마다 생성
        }
    }

    // 꺼내기
    public T Get<T>()
    {
        if (IsEmpty())
        {
            Prepare();
        }

        GameObject go = Pop();
        go.SetActive(true);
        go.transform.SetParent(null);

        return (go.TryGetComponent<T>(out T component)) ? component : default(T);
    }

    // 반환
    public void Return(GameObject go)
    {
        go.SetActive(false);
        go.transform.SetParent(transform);
        Push(go);
    }

    #region 스택
    bool IsEmpty()
    {
        return _pool.Count == 0;
    }

    void Push(GameObject go)
    {
        _pool.Add(go);
    }

    GameObject Pop()
    {
        if (_pool.Count == 0)
            return null;
        GameObject go = _pool.Last();
        _pool.RemoveAt(_pool.Count - 1);
        return go;
    }

    public void Clear()
    {
        foreach (GameObject go in _pool)
        {
            Destroy(go);
        }
        _pool.Clear();
    }
    #endregion
}