using UnityEngine;
using CKT;
using YSJ;

namespace BMC
{
    public class PlayerManager : MonoBehaviour
    {
        static PlayerManager s_instance;
        public static PlayerManager Instance => s_instance;

        public PlayerMove PlayerMove { get; private set; }
        public PlayerDash PlayerDash { get; private set; }
        public PlayerInteract PplayerInteract { get; private set; }
        public PlayerStatus PlayerStatus { get; private set; }
        public PlayerAttack PlayerAttack { get; private set; }
        public CheckPlayerDirection CheckPlayerDirection { get; private set; }

        void Awake()
        {
            if (s_instance == null)
            {
                s_instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            PlayerMove = this.gameObject.GetComponent<PlayerMove>();
            PlayerDash = this.gameObject.GetComponent<PlayerDash>();
            PplayerInteract = this.gameObject.GetComponent<PlayerInteract>();
            PlayerStatus = this.gameObject.GetComponent<PlayerStatus>();
            PlayerAttack = this.gameObject.GetComponent<PlayerAttack>();
            CheckPlayerDirection = this.gameObject.GetComponent<CheckPlayerDirection>();
        }

        void Update()
        {
            if(PlayerStatus.IsDead)
                return;

            CheckPlayerDirection.CheckCurrentDirection();

            //if (Input.GetMouseButtonDown(2)) // 마우스 휠 클릭
            //{
            //    MapManager.Instance.CurrentRoom.RoomClearComplete();
            //}
        }

        void FixedUpdate()
        {
            if(PlayerStatus.IsDead)
                return;

            //_playerMove.enabled = !_playerDash.Silhouette.IsActive;
            if (!PlayerDash.Silhouette.IsActive && !PlayerAttack.IsAttack)
                PlayerMove.Move();
            //else
            //    _playerMove.Stop();

            //if (YSJ.Managers.Input.IsPressLeftHandAttack)
            //{
            //    GameManager.Instance.LeftSkillManager.TriggerHand();
            //}
            ///*else
            //{
            //    GameManager.Instance.LeftSkillManager.OnHandCancelActionT0?.Trigger();
            //}*/

            //if (YSJ.Managers.Input.IsPressRightHandAttack)
            //{
            //    GameManager.Instance.RightSkillManager.TriggerHand();
            //}
            ///*else
            //{
            //    GameManager.Instance.RightSkillManager.OnHandCancelActionT0?.Trigger();
            //}*/
        }

        void OnDestroy()
        {
            s_instance = null;
        }
    }
}