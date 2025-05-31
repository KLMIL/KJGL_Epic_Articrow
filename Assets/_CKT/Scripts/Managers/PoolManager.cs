using System.Collections.Generic;
using UnityEngine;

namespace YSJ
{
    public class PoolManager
    {
        Dictionary<string, List<GameObject>> _poolDic = new Dictionary<string, List<GameObject>>();
        Dictionary<string, GameObject> _prefabDic = new Dictionary<string, GameObject>();

        public void Init()
        {
            _poolDic.Clear();
            _prefabDic.Clear();

            SetPrefab("Prefabs", "Bullet", _prefabDic);
        }

        void SetPrefab(string path, string name, Dictionary<string, GameObject> dic)
        {
            GameObject obj = Resources.Load<GameObject>($"{path}/{name}");
            if (obj == null)
            {
                Debug.LogError($"{name} prefab could not find in Resources folder.");
                return;
            }
            else
            {
                if (!_prefabDic.ContainsKey(name))
                {
                    dic.Add(name, obj);
                    _poolDic.Add(name, new List<GameObject>());
                }
                else
                {
                    Debug.LogError($"{name} prefab already contain.");
                    return;
                }
            }
        }

        //오브젝트 생성
        public GameObject InstPrefab(string name, Transform parent, Vector3 position)
        {
            if (!_prefabDic.ContainsKey(name))
            {
                Debug.Log($"{name} : Prefab Does Not Exist");
                return null;
            }

            GameObject select = null;
            foreach (GameObject item in _poolDic[name])
            {
                if (!item.activeSelf) //발견하면? select 변수에 할당
                {
                    select = item;
                    select.SetActive(true);
                    break;
                }
            }
            if (!select) //못 찾으면? 새롭게 생성하고 >> select 변수에 할당
            {
                select = GameObject.Instantiate(_prefabDic[name]);
                _poolDic[name].Add(select);
            }

            select.transform.parent = null;
            select.transform.position = position;

            return select; //select 반환
        }

        //특정 오브젝트 전체 비활성화
        public void DeleteAllPrefab(string name)
        {
            if (!_prefabDic.ContainsKey(name))
            {
                Debug.Log($"{name} : Does Not Exist");
                return;
            }

            foreach (GameObject item in _poolDic[name]) //선택한 풀의 비활성화된 게임오브젝트에 접근
            {
                if (item.activeSelf) //발견하면? select 변수에 할당
                {
                    item.SetActive(false);
                }
            }
        }
    }
}