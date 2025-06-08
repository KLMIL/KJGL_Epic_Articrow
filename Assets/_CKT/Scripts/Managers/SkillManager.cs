using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CKT
{
    [System.Serializable]
    public class SkillManager
    {
        public Dictionary<string, Func<GameObject, IEnumerator>> CastSkillDict = new Dictionary<string, Func<GameObject, IEnumerator>>();
        public Dictionary<string, Func<GameObject, IEnumerator>> HitSkillDict = new Dictionary<string, Func<GameObject, IEnumerator>>();
        public Dictionary<string, int> SkillDupDict = new Dictionary<string, int>();
        
        public void CheckSkill(List<GameObject> list)
        {
            CastSkillDict.Clear();
            HitSkillDict.Clear();
            SkillDupDict.Clear();

            for (int i = 0; i < list.Count; i++)
            {
                ISkillable skill = list[i].GetComponent<ISkillable>();
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