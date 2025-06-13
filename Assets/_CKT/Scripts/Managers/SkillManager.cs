using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CKT
{
    [System.Serializable]
    public class SkillManager
    {
        #region [OnHandPerformActionT1]
        public ActionT1Handler<List<GameObject>> OnHandPerformActionT1 = new();
        public void TriggerHand()
        {
            OnHandPerformActionT1.Trigger(_slotList);
        }
        #endregion

        #region [OnHandCancelActionT0]
        public ActionT0Handler OnHandCancelActionT0 = new();
        public void TriggerHandCancel()
        {
            OnHandCancelActionT0?.Trigger();
        }
        #endregion

        #region [OnThrowAwayActionT0]
        public ActionT0Handler OnThrowAwayActionT0 = new();
        public void TriggerThrowAway()
        {
            OnThrowAwayActionT0?.Trigger();
        }
        #endregion

        #region [UpdateSlotListActionT1]
        public ActionT1Handler<List<GameObject>> OnUpdateSlotListActionT1 = new();
        public void TriggerUpdateList(List<GameObject> list)
        {
            OnUpdateSlotListActionT1?.Trigger(list);
        }
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
            OnHandPerformActionT1.Init();
            OnHandCancelActionT0.Init();
            OnThrowAwayActionT0.Init();

            OnUpdateSlotListActionT1.Init();
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