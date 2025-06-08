using UnityEngine;

namespace YSJ
{
    public class PlayerController : MonoBehaviour
    {
        PlayerMove _playerMove = new PlayerMove();
        PlayerDash _playerDash = new PlayerDash();
        PlayerAnimator _playerAnimator;
        Rigidbody2D _rigid;

        void Awake()
        {
            _playerAnimator = GetComponent<PlayerAnimator>();
            _rigid = GetComponent<Rigidbody2D>();

            //_playerStatus.A_Dead += ShowRetry;
        }


        void OnEnable()
        {
            //GameManager.Instance.player = this;
        }

        void FixedUpdate()
        {
            _playerMove.Move(Managers.Input.MoveInput, _rigid, _playerAnimator);
        }

        void ShowRetry()
        {
            //UIManager.Instance.CanvasRestart.GetComponent<Canvas>().enabled = true;
        }
    }
}