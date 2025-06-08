using System.Collections;
using UnityEngine;

public class PlayerDash
{
    Rigidbody2D _rb;
    float _dashSpeed = 12.5f;

    private float _dashCoolTime = 0.5f;

    Silhouette _silhouette;
    Coroutine _dashCoroutine;

    void Start()
    {
        //YSJ.Managers.Input.OnRollAction += Dash;
    }

    IEnumerator DashCoroutine(Vector2 direction)
    {
        _silhouette.IsActive = true;                        // 대시 중 실루엣 활성화
        _rb.linearVelocity = direction * _dashSpeed;

        yield return new WaitForSeconds(_dashCoolTime);     // 대시 쿨타임
        _silhouette.IsActive = false;                       // 대시 중 실루엣 비활성화
        _dashCoroutine = null;
    }
}