using UnityEngine;
using YSJ;

namespace BMC
{
    /// <summary>
    /// 플레이어가 문에 닿았는지 감지하고, 감지되면 다음 방으로 이동시키는 클래스
    /// </summary>
    public class DoorDetectionPlayer : MonoBehaviour
    {
        bool _isPlayerInTrigger;
        bool _isTransferPlayer;
        Door _door;
        Collider2D _barrierCollider;

        public void Init(Door door)
        {
            _door = door;
            _barrierCollider = gameObject.GetComponentInDirectChildren<Collider2D>();
        }

        #region OnTrigger
        void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_isPlayerInTrigger && collision.CompareTag("Player") && _door.IsOpen)
            {
                //Debug.Log("문 닿음");
                _isPlayerInTrigger = true;
                PlayerManager.Instance.PlayerTextWindow.SetText("F");
            }
        }

        void OnTriggerStay2D(Collider2D collision)
        {
            if(_isPlayerInTrigger && !_isTransferPlayer && _door.IsOpen && Managers.Input.IsPressInteract)
            {
                //Debug.Log("다음 씬으로 이동");
                _barrierCollider.enabled = false;
                _door.NextStage();
                _isTransferPlayer = true;
                PlayerManager.Instance.PlayerTextWindow.SetText();
            }
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            if (_isPlayerInTrigger && collision.CompareTag("Player") && _door.IsOpen)
            {
                //Debug.Log("문 떠남");
                _isPlayerInTrigger = false;
                PlayerManager.Instance.PlayerTextWindow.SetText();
            }
        }
        #endregion
    }
}