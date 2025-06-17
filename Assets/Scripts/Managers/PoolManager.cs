using System.Collections.Generic;
using UnityEngine;
using static Define;

namespace BMC
{
    /// <summary>
    /// 풀링을 관리하는 클래스
    /// </summary>
    public class PoolManager
    {
        GameObject _root;
        Dictionary<PoolType, List<PoolID>> _poolCategoryDict = new Dictionary<PoolType, List<PoolID>>();
        Dictionary<PoolID, Pool> _poolDict = new Dictionary<PoolID, Pool>();

        public void Init()
        {
            // 풀링 오브젝트를 관리할 루트 오브젝트를 생성
            _root = GameObject.Find("@Pool");
            if (_root == null)
            {
                _root = new GameObject { name = "@Pool" };
                Object.DontDestroyOnLoad(_root);
            }

            InitRegister();
        }

        // 초기 풀링 오브젝트 등록
        public void InitRegister()
        {
            // 풀링 목록 구성
            Register(PoolType.TextPool, PoolID.DamageText, 1000);
            Register(PoolType.EffectPool, PoolID.Mana);
            Register(PoolType.SkillPool, PoolID.Bullet_T1);
            Register(PoolType.SkillPool, PoolID.Bullet_T2);
            Register(PoolType.SkillPool, PoolID.Bullet_T3);
            Register(PoolType.SkillPool, PoolID.CastExplosion);
            Register(PoolType.SkillPool, PoolID.CastDamageArea);
            Register(PoolType.SkillPool, PoolID.HitScatter);
            Register(PoolType.SkillPool, PoolID.HitExplosion);
            Register(PoolType.SkillPool, PoolID.HitDamageArea);
            Register(PoolType.SkillPool, PoolID.GrabObject);
            Register(PoolType.SkillPool, PoolID.Description);
        }

        // 풀링할 오브젝트 등록
        public void Register(PoolType type, PoolID id, int count = 15)
        {
            if (!_poolCategoryDict.ContainsKey(type)) // 새로운 카테고리
            {
                _poolCategoryDict[type] = new List<PoolID>();
                GameObject poolCategory = new GameObject($"{type}");
                poolCategory.transform.SetParent(_root.transform);
            }

            if (!_poolDict.ContainsKey(id)) // 새로운 ID
            {
                _poolCategoryDict[type].Add(id); // 카테고리에 ID 추가

                // 카테고리 하위에 ID에 해당하는 풀 생성
                Transform poolCategoryTransform = _root.transform.Find(type.ToString());
                Pool pool = new GameObject($"{id}Pool").AddComponent<Pool>();
                pool.transform.SetParent(poolCategoryTransform);
                _poolDict[id] = pool;

                // 풀 초기화
                GameObject prefab = Resources.Load<GameObject>($"Prefabs/{type}/{id}");
                pool.Init(prefab);
            }
        }

        // 풀링된 오브젝트 가져오기
        public T Get<T>(PoolID id)
        {
            if (_poolDict.TryGetValue(id, out Pool pool))
            {
                if(typeof(T) == typeof(GameObject))
                {
                    GameObject go = pool.Get<GameObject>();
                    return (T)(object)go; // GameObject로 캐스팅
                }
                else
                {
                    T component = pool.Get<T>();
                    return component;
                }
            }
            else
            {
                Debug.LogError($"Pool of type {id} does not exist.");
                return default(T);
            }
        }

        // 풀링된 오브젝트 반환
        public void Return(PoolID id, GameObject go)
        {
            if (_poolDict.TryGetValue(id, out Pool pool))
            {
                pool.Return(go);
            }
            else
            {
                Debug.LogError($"Pool of type {id} does not exist.");
            }
        }

        public void Clear()
        {
            foreach (var pool in _poolDict.Values)
            {
                pool.Clear();
            }
            _poolDict.Clear();
            _poolCategoryDict.Clear();
        }
    }
}