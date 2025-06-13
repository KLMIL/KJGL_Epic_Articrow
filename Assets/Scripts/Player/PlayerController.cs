using UnityEngine;
using CKT;
using YSJ;
using UnityEditor.SceneManagement;

namespace BMC
{
    public class PlayerController : MonoBehaviour
    {
        PlayerMove _playerMove;
        PlayerDash _playerDash;
        PlayerInteract _playerInteract;

        void Awake()
        {
            _playerMove = this.gameObject.AddComponent<PlayerMove>();
            _playerDash = this.gameObject.AddComponent<PlayerDash>();
            _playerInteract = this.gameObject.AddComponent<PlayerInteract>();
        }

        void Start()
        {
            transform.SetParent(null);
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
            _playerMove.enabled = !_playerDash.Silhouette.IsActive;

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