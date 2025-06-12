using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CKT
{
    [System.Serializable]
    public class SkillManager
    {
        #region [OnHandEvent]
        Action<List<GameObject>> _onHandEvent;
        public void InitHand()
        {
            _onHandEvent = null;
        }
        public void SingleSubHand(Action<List<GameObject>> newSub)
        {
            _onHandEvent = null;
            _onHandEvent += newSub;
        }
        public void InvokeHand()
        {
            _onHandEvent?.Invoke(_list);
        }
        #endregion

        #region [OnHandCancleEvent]
        Action _onHandCancleEvent;
        public void InitHandCancle()
        {
            _onHandCancleEvent = null;
        }
        public void SingleSubHandCancle(Action newSub)
        {
            _onHandCancleEvent = null;
            _onHandCancleEvent += newSub;
        }
        public void InvokeHandCancle()
        {
            _onHandCancleEvent?.Invoke();
        }
        #endregion

        #region [List]
        List<GameObject> _list = new List<GameObject>();
        event Action<List<GameObject>> _onUpdateListEvent;
        public void SubUpdateList(Action<List<GameObject>> newSub)
        {
            _onUpdateListEvent += newSub;
        }
        void InvokeUpdateList(List<GameObject> list)
        {
            _onUpdateListEvent?.Invoke(list);
        }
        #endregion

        #region [SkillDict]
        public Dictionary<string, Func<GameObject, IEnumerator>> CastSkillDict = new Dictionary<string, Func<GameObject, IEnumerator>>();
        public Dictionary<string, Func<GameObject, IEnumerator>> HitSkillDict = new Dictionary<string, Func<GameObject, IEnumerator>>();
        public Dictionary<string, int> SkillDupDict = new Dictionary<string, int>();
        #endregion

        public void Init()
        {
            _onHandEvent = null;
            _onHandCancleEvent = null;

            _list = new List<GameObject>();
            _onUpdateListEvent = null;
        }
        
        public void CheckSkill()
        {
            //슬롯 확인
            InvokeUpdateList(_list);

            //스킬 적용
            CastSkillDict.Clear();
            HitSkillDict.Clear();
            SkillDupDict.Clear();

            for (int i = 0; i < _list.Count; i++)
            {
                ISkillable skill = _list[i].GetComponent<ISkillable>();
                if (skill != null)
                {
                    //알맞은 딕셔너리에 찾기
                    Dictionary<string, Func<GameObject, IEnumerator>> skillDict = null;
                    if (skill.SkillType == SkillType.Cast)
                    {
                        skillDict = CastSkillDict;
                    }
                    else if (skill.SkillType == SkillType.Hit)
                    {
                        skillDict = HitSkillDict;
                    }

                    //딕셔너리에 해당 스킬 할당 || 중복된다면 중복++
                    if (skillDict == null)
                    {
                        Debug.LogError("skillDictionary is null");
                    }
                    else
                    {
                        if (!skillDict.ContainsKey(skill.SkillName))
                        {
                            skillDict.Add(skill.SkillName, (obj) => skill.SkillCoroutine(obj, SkillDupDict[skill.SkillName], this));
                            SkillDupDict.Add(skill.SkillName, 1);
                        }
                        else
                        {
                            SkillDupDict[skill.SkillName]++;
                        }
                    }
                }
            }
        }
    }
}