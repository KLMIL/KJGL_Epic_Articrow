using BMC;
using System.Collections;
using UnityEngine;

public class Stun : Debuff
{
    PlayerDebuff _playerDebuff;
    Coroutine _coroutine;

    void Awake()
    {
        _playerDebuff = PlayerManager.Instance.transform.GetComponent<PlayerDebuff>();
    }

    public override void Apply(float duration, float damage = 0f, float interval = 0f)
    {
        if (PlayerManager.Instance.PlayerDash.IsDash)
            return;

        // 이미 스턴 중인 경우, 기존 코루틴을 중지하고 새로 시작
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        _coroutine = StartCoroutine(StunCoroutine(duration));
    }

    IEnumerator StunCoroutine(float duration)
    {
        Debug.Log("스턴 적용");
        _playerDebuff.CurrentDebuff |= DebuffType.Stun;
        yield return new WaitForSeconds(duration);
        _playerDebuff.CurrentDebuff &= ~DebuffType.Stun;
        _coroutine = null;
        Debug.Log("스턴 해제");
    }
}