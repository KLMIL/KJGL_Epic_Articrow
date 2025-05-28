using UnityEngine;

public class PlayerController : MonoBehaviour
{
    MagicHand _magicHand;
    PlayerStatus _playerStatus;
    PlayerMove _playerMove;

    void Awake()
    {
        _magicHand = GetComponent<MagicHand>();
        _playerStatus = GetComponent<PlayerStatus>();
        _playerMove = GetComponent<PlayerMove>();

        _playerStatus.A_Dead += ShowRetry;
    }


    void OnEnable()
    {
        GameManager.Instance.player = this;
    }


    void Update()
    {
        UIManager.Instance.inventoryStat_MaxHealth.TextUpdate(_playerStatus.MaxHealth);
        UIManager.Instance.inventoryStat_MaxMana.TextUpdate(_playerStatus.MaxMana);
        UIManager.Instance.inventoryStat_ManaRecovery.TextUpdate(_playerStatus.ManaRecoverySpeed);
        UIManager.Instance.inventoryStat_MoveSpeed.TextUpdate(_playerStatus.MoveSpeed);
        UIManager.Instance.inventoryStat_L_CoolTime.TextUpdate(_magicHand.L_CoolTime);
        UIManager.Instance.InventoryStat_R_CoolTime.TextUpdate(_magicHand.R_CoolTime);
    }

    void FixedUpdate()
    {
        _playerMove.Move(_playerStatus.MoveSpeed);
    }

    void ShowRetry() 
    {
        UIManager.Instance.CanvasRestart.GetComponent<Canvas>().enabled = true;
    }
}
