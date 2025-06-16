using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace YSJ
{
    [System.Serializable]
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
            SetPrefab("Prefabs", "Bullet_T1", _prefabDict);
            SetPrefab("Prefabs", "Bullet_T2", _prefabDict);
            SetPrefab("Prefabs", "Bullet_T3", _prefabDict);

            SetPrefab("Prefabs", "HitSkillObject", _prefabDict);

            SetPrefab("Prefabs", "CastExplosion", _prefabDict);
            SetPrefab("Prefabs", "CastDamageArea", _prefabDict);

            SetPrefab("Prefabs", "HitScatter", _prefabDict);
            SetPrefab("Prefabs", "HitExplosion", _prefabDict);
            SetPrefab("Prefabs", "HitDamageArea", _prefabDict);
            SetPrefab("Prefabs", "GrabObject", _prefabDict);

            SetPrefab("Prefabs", "Description", _prefabDict);
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

            //삭제된 오브젝트 리스트에서 삭제
            _poolDict[name] = _poolDict[name].Where(obj => (obj != null)).ToList();

            //리스트에 비활성화된 오브젝트 있는지 찾기
            GameObject select = null;
            foreach (GameObject item in _poolDict[name])
            {
                if (!item)
                {
                    Debug.LogWarning($"{name} list has null object");
                    continue;
                }
                else
                {
                    if (!item.activeSelf)
                    {
                        select = item;
                        select.SetActive(true);
                        break;
                    }
                }
            }

            //리스트에 비활성화된 오브젝트가 없다면 새롭게 생성
            if (!select) 
            {
                select = GameObject.Instantiate(_prefabDict[name]);
                select.transform.SetParent(_poolParent.transform);
                _poolDict[name].Add(select);
            }

            return select;
        }

        //특정 오브젝트 전체 비활성화
        public void DeleteAllPrefab(string name)
        {
            if (!_prefabDict.ContainsKey(name))
            {
                Debug.Log($"{name} : Does Not Exist");
                return;
            }

            foreach (GameObject item in _poolDict[name])
            {
                if (item.activeSelf)
                {
                    item.SetActive(false);
                }
            }
        }
    }
}