using UnityEngine;
using CKT;
using YSJ;

namespace BMC
{
    public class PlayerManager : MonoBehaviour
    {
        static PlayerManager s_instance;
        public static PlayerManager Instance => s_instance;

        PlayerMove _playerMove;
        PlayerDash _playerDash;
        PlayerInteract _playerInteract;
        PlayerStatus _playerStatus;
        PlayerAttack _playerAttack;

        void Awake()
        {
            if (s_instance == null)
            {
                s_instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            _playerMove = this.gameObject.AddComponent<PlayerMove>();
            _playerDash = this.gameObject.AddComponent<PlayerDash>();
            _playerInteract = this.gameObject.AddComponent<PlayerInteract>();
            _playerStatus = this.gameObject.AddComponent<PlayerStatus>();
            _playerAttack = this.gameObject.AddComponent<PlayerAttack>();
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(2)) // 마우스 휠 클릭
            {
                MapManager.Instance.CurrentRoom.Complete();
            }
        }

        void FixedUpdate()
        {
            //_playerMove.enabled = !_playerDash.Silhouette.IsActive;
            if (!_playerDash.Silhouette.IsActive && !_playerAttack.IsAttack)
                _playerMove.Move();
            //else
            //    _playerMove.Stop();

            if (YSJ.Managers.Input.IsPressLeftHandAttack)
            {
                GameManager.Instance.LeftSkillManager.TriggerHand();
            }
            /*else
            {
                GameManager.Instance.LeftSkillManager.OnHandCancelActionT0?.Trigger();
            }*/

            if (YSJ.Managers.Input.IsPressRightHandAttack)
            {
                GameManager.Instance.RightSkillManager.TriggerHand();
            }
            /*else
            {
                GameManager.Instance.RightSkillManager.OnHandCancelActionT0?.Trigger();
            }*/
        }
    }
}