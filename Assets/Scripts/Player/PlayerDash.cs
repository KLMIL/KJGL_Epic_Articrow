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
        public float DashCoolTime { get; private set; } = 1f;
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

            _dashCoroutine = _dashCoroutine ?? StartCoroutine(DashCoroutine(dashDir, _dashSpeed, _dashTime, DashCoolTime));
        }

        IEnumerator DashCoroutine(Vector2 dashDir, float dashSpeed, float dashTime, float dashCoolTime)
        {
            _silhouette.IsActive = true;
            IsDash = true;
            _rb.linearVelocity += dashDir * dashSpeed;

            yield return new WaitForSeconds(dashTime);
            _silhouette.IsActive = false;
            IsDash = false;
            _rb.linearVelocity -= _rb.linearVelocity;

            float remainCoolTime = dashCoolTime - dashTime;
            float timer = 0;
            UI_InGameEventBus.OnPlayerDashCoolTimeSliderValueUpdate?.Invoke(timer);
            while (timer < remainCoolTime)
            {
                timer += Time.deltaTime;
                UI_InGameEventBus.OnPlayerDashCoolTimeSliderValueUpdate?.Invoke(timer);
                yield return null;
            }
            UI_InGameEventBus.OnPlayerDashCoolTimeSliderValueUpdate?.Invoke(DashCoolTime);
            _dashCoroutine = null;
        }

        #region 대시 마법 파츠
        public void DashSkill(Vector2 dashDir)
        {
            StartCoroutine(DashSkillCoroutine(dashDir));
        }

        IEnumerator DashSkillCoroutine(Vector2 dashDir)
        {
            _silhouette.IsActive = true;
            //_hitBox.enabled = false;
            IsDash = true;
            _rb.linearVelocity += dashDir * _dashSpeed;

            yield return new WaitForSeconds(_dashTime);
            _silhouette.IsActive = false;
            IsDash = false;
            //_hitBox.enabled = true;
            _rb.linearVelocity -= _rb.linearVelocity;
        }
        #endregion

        void OnDestroy()
        {
            _silhouette.Clear(); // 실루엣 정리
            YSJ.Managers.Input.OnDashAction -= TryDash;
        }
    }
}