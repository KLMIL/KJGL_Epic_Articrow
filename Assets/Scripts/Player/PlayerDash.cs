using System.Collections;
using UnityEngine;

namespace BMC
{
    public class PlayerDash : MonoBehaviour
    {
        Rigidbody2D _rb;
        public Silhouette Silhouette => _silhouette;
        Silhouette _silhouette;

        Coroutine _dashCoroutine;
        float _dashSpeed = 14f;
        float _dashTime = 0.15f;
        public float DashCoolTime { get; set; }
        [field: SerializeField] public bool IsDash { get; private set; } = false;

        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _silhouette = GetComponent<Silhouette>();
            YSJ.Managers.Input.OnDashAction += TryDash;
        }

        public void TryDash(Vector2 dashDir)
        {
            if(PlayerManager.Instance.PlayerStatus.IsDead)
                return;

            _dashCoroutine = _dashCoroutine ?? StartCoroutine(DashCoroutine(dashDir));
        }

        IEnumerator DashCoroutine(Vector2 dashDir)
        {
            _silhouette.IsActive = true;
            IsDash = true;
            _rb.linearVelocity += dashDir * _dashSpeed;

            yield return new WaitForSeconds(_dashTime);
            _silhouette.IsActive = false;
            IsDash = false;
            _rb.linearVelocity -= _rb.linearVelocity;

            //float remainCoolTime = DashCoolTime - _dashTime;
            float timer = 0;
            UI_InGameEventBus.OnPlayerDashCoolTimeSliderValueUpdate?.Invoke(timer);
            while (timer < DashCoolTime)
            {
                timer += Time.deltaTime;
                UI_InGameEventBus.OnPlayerDashCoolTimeSliderValueUpdate?.Invoke(timer);
                yield return null;
            }
            UI_InGameEventBus.OnPlayerDashCoolTimeSliderValueUpdate?.Invoke(DashCoolTime);
            _dashCoroutine = null;
        }

        void OnDestroy()
        {
            _silhouette.Clear(); // 실루엣 정리
            YSJ.Managers.Input.OnDashAction -= TryDash;
        }
    }
}