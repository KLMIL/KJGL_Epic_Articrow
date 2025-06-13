using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CKT
{
    [System.Serializable]
    public class SkillManager
    {
        #region [OnHandActionT1]
        public ActionT1Handler<List<GameObject>> OnHandActionT1 = new();
        public void TriggerHand()
        {
            OnHandActionT1.Trigger(_slotList);
        }
        #endregion

        #region [OnHandCancelActionT0]
        public ActionT0Handler OnHandCancelActionT0 = new();
        public void TriggerHandCancel()
        {
            OnHandCancelActionT0?.Trigger();
        }
        #endregion

        #region [UpdateListActionT1]
        public ActionT1Handler<List<GameObject>> OnUpdateListActionT1 = new();
        public void TriggerUpdateList(List<GameObject> list)
        {
            OnUpdateListActionT1?.Trigger(list);
        }

        /*event Action<List<GameObject>> _onUpdateListEvent;
        public void SubUpdateList(Action<List<GameObject>> newSub)
        {
            _onUpdateListEvent += newSub;
        }
        void InvokeUpdateList(List<GameObject> list)
        {
            _onUpdateListEvent?.Invoke(list);
        }*/
        #endregion
        
        #region [SlotList]
        List<GameObject> _slotList = new List<GameObject>();
        #endregion

        #region [SkillDict]
        public Dictionary<string, Func<GameObject, IEnumerator>> CastSkillDict = new Dictionary<string, Func<GameObject, IEnumerator>>();
        public Dictionary<string, Func<GameObject, IEnumerator>> HitSkillDict = new Dictionary<string, Func<GameObject, IEnumerator>>();
        public Dictionary<string, int> SkillDupDict = new Dictionary<string, int>();
        #endregion

        public void Init()
        {
            OnHandActionT1.Init();
            OnHandCancelActionT0.Init();
            OnUpdateListActionT1.Init();
        }

        #region [CheckSkill]
        public void CheckSkill()
        {
            //슬롯 확인
            TriggerUpdateList(_slotList);

            //스킬 적용
            CastSkillDict.Clear();
            HitSkillDict.Clear();
            SkillDupDict.Clear();

            for (int i = 0; i < _slotList.Count; i++)
            {
                ISkillable skill = _slotList[i].GetComponent<ISkillable>();
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
        #endregion
    }
}