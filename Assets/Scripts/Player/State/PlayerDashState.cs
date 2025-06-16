using System.Collections;
using UnityEngine;
using YSJ;

namespace BMC
{
    public class PlayerDashState : State
    {
        PlayerFSM _playerFSM;
        Animator _anim;
        Rigidbody2D _rb;
        CheckPlayerDirection _checkPlayerDirection;

        public Silhouette Silhouette => _silhouette;
        Silhouette _silhouette;

        Coroutine _dashCoroutine;
        float _dashSpeed = 14f;
        float _dashTime = 0.15f;
        float _dashCoolTime = 0.4f;

        [field: SerializeField] public bool IsDash { get; private set; } = false;

        void Start()
        {
            _anim = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody2D>();
            _playerFSM = GetComponent<PlayerFSM>();
            _checkPlayerDirection = GetComponent<CheckPlayerDirection>();

            _silhouette = GetComponent<Silhouette>();
            YSJ.Managers.Input.OnDashAction += Dash;
        }

        public override void OnStateEnter() 
        { 
        }

        public override void OnStateUpdate()
        {
            if (_dashCoroutine == null)
            {
                _playerFSM.CheckAndSetMove();
                _playerFSM.CheckAndSetIdle();
            }
        }

        public override void OnStateExit() 
        {
        }

        public void Dash(Vector2 dashDir)
        {
            _dashCoroutine = _dashCoroutine ?? StartCoroutine(DashCoroutine(dashDir, _dashSpeed, _dashTime, _dashCoolTime));
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
            while (timer < remainCoolTime)
            {
                timer += Time.deltaTime;
            }
            _dashCoroutine = null;
        }

        void OnDestroy()
        {
            YSJ.Managers.Input.OnDashAction -= Dash;
        }
    }
}