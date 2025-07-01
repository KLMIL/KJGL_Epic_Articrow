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

    public override void Do(float time)
    {
        // 이미 스턴 중인 경우, 기존 코루틴을 중지하고 새로 시작
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        _coroutine = StartCoroutine(StunCoroutine(time));
    }

    IEnumerator StunCoroutine(float time)
    {
        _playerDebuff.CurrentDebuff |= DebuffType.Stun;
        _playerDebuff.IsStun = true;
        yield return new WaitForSeconds(time);
        _playerDebuff.CurrentDebuff &= ~DebuffType.Stun;
        _playerDebuff.IsStun = false;
        _coroutine = null;
        Debug.Log("스턴 해제");
    }
}