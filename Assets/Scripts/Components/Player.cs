using UnityEngine;

public class Player : MonoBehaviour
{
    MagicHand magicHand;

    private void Awake()
    {
        magicHand = GetComponent<MagicHand>();
        if (TryGetComponent<Health>(out Health health)) 
        {
            health.A_Dead += ShowRetry;
        }
    }


    private void OnEnable()
    {
        GameManager.Instance.player = gameObject;
    }


    private void Update()
    {
        UIManager.Instance.inventoryStat_MaxHealth.TextUpdate(GetComponent<Health>().maxHealth);
        UIManager.Instance.inventoryStat_MaxMana.TextUpdate(GetComponent<Mana>().maxMana);
        UIManager.Instance.inventoryStat_ManaRecovery.TextUpdate(GetComponent<Mana>().ManaRecovery);
        UIManager.Instance.inventoryStat_MoveSpeed.TextUpdate(GetComponent<Move>().speed);
        UIManager.Instance.inventoryStat_L_CoolTime.TextUpdate(GetComponent<MagicHand>().L_CoolTime);
        UIManager.Instance.InventoryStat_R_CoolTime.TextUpdate(GetComponent<MagicHand>().R_CoolTime);
    }

    void ShowRetry() 
    {
        UIManager.Instance.CanvasRestart.GetComponent<Canvas>().enabled = true;
    }
}
