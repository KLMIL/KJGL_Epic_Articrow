using System.Collections.Generic;
using UnityEngine;

namespace YSJ
{
    public class PoolManager
    {
        GameObject _poolParent;
        
        Dictionary<string, List<GameObject>> _poolDict = new Dictionary<string, List<GameObject>>();
        Dictionary<string, GameObject> _prefabDict = new Dictionary<string, GameObject>();

        public void Init()
        {
            _poolParent = new GameObject("PoolParent");
            GameObject.DontDestroyOnLoad(_poolParent);
            
            _poolDict.Clear();
            _prefabDict.Clear();
            SetPrefab("Prefabs", "Bullet", _prefabDict);
            SetPrefab("Prefabs", "HitSkillObject", _prefabDict);

            SetPrefab("Prefabs", "CastExplosion", _prefabDict);
            SetPrefab("Prefabs", "CastDamageArea", _prefabDict);

            SetPrefab("Prefabs", "HitScatter", _prefabDict);
            SetPrefab("Prefabs", "HitExplosion", _prefabDict);
            SetPrefab("Prefabs", "HitDamageArea", _prefabDict);
            SetPrefab("Prefabs", "GrabObject", _prefabDict);
        }

        void SetPrefab(string path, string name, Dictionary<string, GameObject> dict)
        {
            GameObject obj = Resources.Load<GameObject>($"{path}/{name}");
            if (obj == null)
            {
                Debug.LogError($"{name} prefab could not find in Resources folder.");
                return;
            }
            else
            {
                if (!_prefabDict.ContainsKey(name))
                {
                    dict.Add(name, obj);
                    _poolDict.Add(name, new List<GameObject>());
                }
                else
                {
                    Debug.LogError($"{name} prefab already contain.");
                    return;
                }
            }
        }

        //오브젝트 생성
        public GameObject InstPrefab(string name)
        {
            if (!_prefabDict.ContainsKey(name))
            {
                Debug.Log($"{name} : Prefab Does Not Exist");
                return null;
            }

            GameObject select = null;
            foreach (GameObject item in _poolDict[name])
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
                select = GameObject.Instantiate(_prefabDict[name]);
                select.transform.SetParent(_poolParent.transform);
                _poolDict[name].Add(select);
            }

            return select; //select 반환
        }

        //특정 오브젝트 전체 비활성화
        public void DeleteAllPrefab(string name)
        {
            if (!_prefabDict.ContainsKey(name))
            {
                Debug.Log($"{name} : Does Not Exist");
                return;
            }

            foreach (GameObject item in _poolDict[name]) //선택한 풀의 비활성화된 게임오브젝트에 접근
            {
                if (item.activeSelf) //발견하면? select 변수에 할당
                {
                    item.SetActive(false);
                }
            }
        }
    }
}