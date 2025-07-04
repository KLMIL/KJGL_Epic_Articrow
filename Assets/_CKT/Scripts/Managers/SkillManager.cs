using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CKT
{
    [System.Serializable]
    public class SkillManager
    {
        #region [아티팩트가 생성할 Projectile 스탯]
        public float DamageRate = 1f;
        public FuncT0<ArtifactSO> GetArtifactSOFuncT0 = new();
        #endregion

        #region [아티팩트 공격 실행]
        public ActionT1<List<GameObject>> OnHandPerformActionT1 = new();
        public void TriggerHand()
        {
            OnHandPerformActionT1.Publish(_slotList);
        }
        #endregion

        #region [아티팩트 공격 취소]
        public ActionT0 OnHandCancelActionT0 = new();
        #endregion

        #region [아티팩트 버리기 실행]
        public ActionT0 OnThrowAwayActionT0 = new();
        #endregion

        #region [슬롯 내용물 갱신]
        public ActionT1<List<GameObject>> OnUpdateSlotListActionT1 = new();
        #endregion
        
        #region [SlotList]
        public List<GameObject> SlotList => _slotList;
        List<GameObject> _slotList = new List<GameObject>();
        #endregion

        #region [SkillDict]
        public Dictionary<string, Func<Vector3, Vector3, IEnumerator>> CastSkillDict = new Dictionary<string, Func<Vector3, Vector3, IEnumerator>>();
        public Dictionary<string, Func<Vector3, Vector3, IEnumerator>> HitSkillDict = new Dictionary<string, Func<Vector3, Vector3, IEnumerator>>();
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
            OnUpdateSlotListActionT1?.Publish(_slotList);

            //스킬 적용
            CastSkillDict.Clear();
            HitSkillDict.Clear();
            SkillDupDict.Clear();

            for (int i = 0; i < _slotList.Count; i++)
            {
                ImageParts imageParts = _slotList[i].GetComponent<ImageParts>();
                ISkillable skill = _slotList[i].GetComponent<ISkillable>();
                if ((imageParts != null) && (skill != null))
                {
                    //알맞은 딕셔너리에 찾기
                    Dictionary<string, Func<Vector3, Vector3, IEnumerator>> skillDict = null;
                    if (imageParts.SkillType == Define.SkillType.Cast)
                    {
                        skillDict = CastSkillDict;
                    }
                    else if (imageParts.SkillType == Define.SkillType.Hit)
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
                        if (!skillDict.ContainsKey(imageParts.SkillName))
                        {
                            skillDict.Add(imageParts.SkillName, (position, direction) => skill.SkillCoroutine(position, direction, SkillDupDict[imageParts.SkillName], this));
                            SkillDupDict.Add(imageParts.SkillName, 1);
                        }
                        else
                        {
                            SkillDupDict[imageParts.SkillName]++;
                        }
                    }
                }
            }

            int castScatterCount = 1;
            if (SkillDupDict.ContainsKey("CastScatter"))
            {
                castScatterCount = 1 + (2 * SkillDupDict["CastScatter"]);
            }

            int hitScatterCount = 1;
            if (SkillDupDict.ContainsKey("HitScatter"))
            {
                hitScatterCount = 2 + SkillDupDict["HitScatter"];
            }

            int castAdditional = 1;
            if (SkillDupDict.ContainsKey("CastAdditional"))
            {
                castAdditional = 1 + SkillDupDict["CastAdditional"];
            }

            int totalProjectileCount = castScatterCount * hitScatterCount * castAdditional;
            DamageRate = Mathf.Ceil(100 * (1f / totalProjectileCount)) / 100f;
        }
        #endregion
    }
}