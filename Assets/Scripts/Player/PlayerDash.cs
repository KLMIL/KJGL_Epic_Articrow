using System.Collections;
using UnityEngine;

namespace BMC
{
    public class PlayerDash : MonoBehaviour
    {
        Collider2D _hitBox;
        Rigidbody2D _rb;
        public Silhouette Silhouette => _silhouette;
        Silhouette _silhouette;

        Coroutine _dashCoroutine;
        float _dashSpeed = 14f;
        float _dashTime = 0.15f;
        float _dashCoolTime = 0.4f;

        void Start()
        {
            Collider2D[] _colliderArray = GetComponents<Collider2D>();
            for (int i = 0; i < _colliderArray.Length; i++)
            {
                if (_colliderArray[i].isTrigger)
                {
                    _hitBox = _colliderArray[i];
                }
            }
            _rb = GetComponent<Rigidbody2D>();
            _silhouette = GetComponent<Silhouette>();
            YSJ.Managers.Input.OnDashAction += Dash;
        }

        public void Dash(Vector2 dashDir)
        {
            _dashCoroutine = _dashCoroutine ?? StartCoroutine(DashCoroutine(dashDir, _dashSpeed, _dashTime, _dashCoolTime));
        }

        IEnumerator DashCoroutine(Vector2 dashDir, float dashSpeed, float dashTime, float dashCoolTime)
        {
            _silhouette.IsActive = true;
            _hitBox.enabled = false;
            _rb.linearVelocity += dashDir * dashSpeed;

            yield return new WaitForSeconds(dashTime);
            _silhouette.IsActive = false;
            _hitBox.enabled = true;
            _rb.linearVelocity -= _rb.linearVelocity;

            float remainCoolTime = dashCoolTime - dashTime;
            float timer = 0;
            while (timer < remainCoolTime)
            {
                timer += Time.deltaTime;
            }
            _dashCoroutine = null;
        }
    }
}