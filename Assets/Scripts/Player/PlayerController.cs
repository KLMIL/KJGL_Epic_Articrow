using UnityEngine;

namespace YSJ
{
    public class PlayerController : MonoBehaviour
    {
        PlayerMove _playerMove;

        void Awake()
        {
            _playerMove = GetComponent<PlayerMove>();

            //_playerStatus.A_Dead += ShowRetry;
        }


        void OnEnable()
        {
            //GameManager.Instance.player = this;
        }

        void FixedUpdate()
        {
            _playerMove.Move();
        }

        void ShowRetry()
        {
            //UIManager.Instance.CanvasRestart.GetComponent<Canvas>().enabled = true;
        }
    }
}