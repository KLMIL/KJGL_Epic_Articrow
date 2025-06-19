using System.Collections.Generic;
using UnityEngine;

namespace BMC
{
    // 패시브 시스템, 스킬 시스템 리팩토링할 때 병합해야 함
    public class PassiveSystem : MonoBehaviour
    {
        Dictionary<PassiveSkill, int> _passiveSkillDict = new (); // 패시브 스킬 종류별 목록

        void Init()
        {

        }

        // 패시브 스킬 추가 (아티팩트 슬롯에 넣는 순간 1번 적용)
        public void AddPassiveSkill(PassiveSkill passiveSkill)
        {
            if (!_passiveSkillDict.ContainsKey(passiveSkill))
            {
                _passiveSkillDict.Add(passiveSkill, 1);
            }
            else
            {
                _passiveSkillDict[passiveSkill]++;
            }
            passiveSkill.Apply();
        }

        // 패시브 스킬 제거 (아티팩트 슬롯에서 제거하는 순간 1번 적용)
        public void RemovePassiveSkill(PassiveSkill passiveSkill)
        {
            if (!_passiveSkillDict.ContainsKey(passiveSkill))
            {
                return;
            }
            else
            {
                _passiveSkillDict[passiveSkill]--;
                if(_passiveSkillDict[passiveSkill] <= 0)
                {
                    _passiveSkillDict.Remove(passiveSkill);
                }
            }
            passiveSkill.Remove();
        }
    }
}