using BMC;
using System.Collections;
using UnityEngine;

public class Burn : Debuff
{
    PlayerDebuff _playerDebuff;
    Coroutine _coroutine;
    PlayerHurt _playerHurt;

    void Awake()
    {
        _playerDebuff = PlayerManager.Instance.PlayerDebuff;
        _playerHurt = PlayerManager.Instance.PlayerHurt;
    }

    public override void Apply(float duration, float damage = 0f, float interval = 0f)
    {
        // 이미 스턴 중인 경우, 기존 코루틴을 중지하고 새로 시작
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        _coroutine = StartCoroutine(StunCoroutine(duration, damage, interval));
    }

    IEnumerator StunCoroutine(float duration, float damage, float interval)
    {
        Debug.Log("화상 적용");
        _playerDebuff.CurrentDebuff |= DebuffType.Burn;

        // 화상 효과: 1초마다 damage 데미지
        float timer = 0f;
        while(timer < duration)
        {
            _playerHurt.TakeDamage(damage);
            yield return new WaitForSeconds(interval);
            timer += interval;

            Debug.Log(timer + "초 경과, 화상 데미지: " + damage);
        }
        _playerDebuff.CurrentDebuff &= ~DebuffType.Burn;
        _coroutine = null;
        Debug.Log("화상 해제");
    }
}