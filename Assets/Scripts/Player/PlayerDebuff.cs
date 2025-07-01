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
        Poison = 1 << 1,    // 중독
        Burn = 1 << 2,      // 화상
        Stun = 1 << 3,      // 기절
    }

    public class PlayerDebuff : MonoBehaviour
    {
        public bool IsStun { get; set; }

        public DebuffType CurrentDebuff { get; set; } = DebuffType.None;

        // 각 디버프별 지속시간 코루틴 관리
        Dictionary<DebuffType, Coroutine> _activeDebuffCoroutines = new Dictionary<DebuffType, Coroutine>();

        // 디버프 적용
        public void ApplyDebuff(DebuffType debuff, float duration)
        {
            if (debuff == DebuffType.None)
                return;

            // 이미 같은 디버프 적용 중이면 타이머 리셋
            if (_activeDebuffCoroutines.TryGetValue(debuff, out var running))
                StopCoroutine(running);

            // 플래그 추가
            CurrentDebuff |= debuff;

            // 지속 시간 관리
            Coroutine debuffCoroutine = StartCoroutine(DebuffDurationCoroutine(debuff, duration));
            _activeDebuffCoroutines[debuff] = debuffCoroutine;
        }

        // 디버프별 지속 시간 & 효과 관리
        IEnumerator DebuffDurationCoroutine(DebuffType debuff, float duration)
        {
            // Stun: 이동 컴포넌트 비활성화
            if (debuff == DebuffType.Stun)
            {
                //var mover = GetComponent<PlayerMovement>();
                //if (mover != null) 
                //    mover.enabled = false;
            }

            // Burn: 매초 데미지
            Coroutine burnEffect = null;
            if (debuff == DebuffType.Burn)
                burnEffect = StartCoroutine(BurnEffectCoroutine());

            // 지속 시간 동안 대기
            yield return new WaitForSeconds(duration);

            /* 디버프 해제 */

            // 플래그 제거
            CurrentDebuff &= ~debuff;
            
            // 코루틴 제거
            _activeDebuffCoroutines.Remove(debuff);

            // Burn 코루틴 정지
            if (burnEffect != null)
                StopCoroutine(burnEffect);

            //// Stun 해제: 이동 컴포넌트 활성화
            //if (debuff == DebuffType.Stun)
            //{
            //    var mover = GetComponent<PlayerMovement>();
            //    if (mover != null) mover.enabled = true;
            //}
        }

        // 화상(Burn) 디버프 효과: 1초마다 5 데미지
        private IEnumerator BurnEffectCoroutine()
        {
            var status = GetComponent<PlayerStatus>();
            while (true)
            {
                status?.TakeDamage(5);
                yield return new WaitForSeconds(1f);
            }
        }

        /// <summary>
        /// 현재 디버프가 적용 중인지 확인
        /// </summary>
        public bool HasDebuff(DebuffType debuff)
        {
            return (CurrentDebuff & debuff) != 0;
        }
    }
}