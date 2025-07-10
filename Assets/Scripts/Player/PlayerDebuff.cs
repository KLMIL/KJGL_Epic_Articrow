using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YSJ;

namespace BMC
{
    [Flags]
    public enum DebuffType
    {
        None = 0,
        Stun = 1 << 1,      // 기절
        Poison = 1 << 2,    // 중독
        Burn = 1 << 3,      // 화상
    }

    public class PlayerDebuff : MonoBehaviour
    {
        [field: SerializeField] public DebuffType CurrentDebuff { get; set; } = DebuffType.None;

        Dictionary<DebuffType, Debuff> _debuffDict = new Dictionary<DebuffType, Debuff>();

        void Awake()
        {
            // 디버프 초기화
            _debuffDict.Add(DebuffType.Stun, GetComponentInChildren<Stun>());
            _debuffDict.Add(DebuffType.Burn, GetComponentInChildren<Burn>());
        }

        void Update()
        {
            TestCode();
        }

        // 디버프 적용
        public void ApplyDebuff(DebuffType debuffType, float duration, float damage = 0f, float interval = 0f)
        {
            if (debuffType == DebuffType.None)
                return;

            // 이미 같은 디버프 적용 중이면 중지 (새로 시작)
            if (_debuffDict.TryGetValue(debuffType, out var debuff))
            {
                _debuffDict[debuffType].Apply(duration, damage, interval);
            }
        }

        /// <summary>
        /// 현재 디버프가 적용 중인지 확인
        /// </summary>
        public bool HasDebuff(DebuffType debuffType)
        {
            return (CurrentDebuff & debuffType) != 0;
        }

        public void TestCode()
        {
            //if (Input.GetKeyDown(KeyCode.Z))
            //{
            //    //ApplyDebuff(DebuffType.Stun, 2f);
            //    //ApplyDebuff(DebuffType.Burn, 6f, 1f, 1f);
            //}
        }
    }
}