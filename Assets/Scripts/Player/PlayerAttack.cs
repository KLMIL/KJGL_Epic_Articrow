using UnityEngine;
using System.Collections;
using YSJ;

namespace BMC
{
    public class PlayerAttack : MonoBehaviour
    {
        //Animator _anim;
        Coroutine _attackCoroutine;

        AttackSlash _attackSlash;

        [field: SerializeField] public bool IsAttack { get; private set; }
        void Start()
        {
            //_anim = GetComponent<Animator>();
            _attackSlash = GetComponentInChildren<AttackSlash>();
            Managers.Input.OnLeftHandAction += Attack;
        }

        void Attack()
        {
            if (GameManager.Instance.IsPaused)
                return;

            Debug.Log("Attack 시작");
            StartAttackCoroutine();

            Debug.Log("Attack 종료");
            IsAttack = false;
        }

        public void StartAttackCoroutine()
        {
            if (_attackCoroutine == null)
                _attackCoroutine = StartCoroutine(AttackCoroutine());
        }

        IEnumerator AttackCoroutine()
        {
            IsAttack = true;

            Vector3 mousePos = Managers.Input.MouseWorldPos;
            Vector2 dir = (mousePos - transform.position).normalized;
            Debug.Log(dir);

            // TODO: 플레이어가 공격 방향 보게 강제로 바꾸기
            //if (dir.x != 0)
            //    _playerFSM.Flip(dir.x);

            //_anim.SetTrigger("AttackTrigger");
            _attackSlash.Play();

            // 애니메이션 길이만큼 대기 (예: 0.5초)
            yield return new WaitForSeconds(0.5f);

            IsAttack = false;
            _attackCoroutine = null;
        }
    }
}