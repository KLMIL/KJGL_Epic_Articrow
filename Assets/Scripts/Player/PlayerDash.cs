using System.Collections;
using UnityEngine;

namespace BMC
{
    public class PlayerDash : MonoBehaviour
    {
        Rigidbody2D _rb;
        Silhouette _silhouette;

        public Silhouette Silhouette => _silhouette;

        Coroutine _dashCoroutine;
        float _dashSpeed = 10f;
        float _dashTime = 0.15f;
        float _dashCoolTime = 0.4f;

        void Start()
        {
            YSJ.Managers.Input.OnDashAction += Dash;
            _rb = GetComponent<Rigidbody2D>();
            _silhouette = GetComponent<Silhouette>();
        }

        public void Dash(Vector2 dashDir)
        {
            _dashCoroutine = _dashCoroutine ?? StartCoroutine(DashCoroutine(dashDir, _dashSpeed, _dashTime, _dashCoolTime));
        }

        IEnumerator DashCoroutine(Vector2 dashDir, float dashSpeed, float dashTime, float dashCoolTime)
        {
            _silhouette.IsActive = true;
            _rb.linearVelocity += dashDir * dashSpeed;

            yield return new WaitForSeconds(dashTime);
            _silhouette.IsActive = false;
            _rb.linearVelocity -= _rb.linearVelocity;

            float remainCoolTime = dashCoolTime - dashTime;
            yield return new WaitForSeconds(remainCoolTime);
            _dashCoroutine = null;
        }
    }
}